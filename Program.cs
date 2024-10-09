using GarageDoorsWeb;
using GarageDoorsWeb.Repositories.Contacts;
using GarageDoorsWeb.Repositories;
using Microsoft.AspNetCore.Builder;
using GarageDoorsWeb.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace GarageDoorsWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();

            var jwtSettings = builder.Configuration.GetSection("Jwt");
            var key = Encoding.UTF8.GetBytes(jwtSettings.GetValue<string>("Secret"));

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                        {
                            context.Response.Headers.Add("Token-Expired", "true");
                        }
                        return Task.CompletedTask;
                    },
                    OnChallenge = context =>
                    {
                        // Skip the default logic.
                        context.HandleResponse();
                        context.Response.Redirect("/Login");
                        return Task.CompletedTask;
                    }
                };
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.GetValue<string>("Issuer"),
                    ValidAudience = jwtSettings.GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });

            builder.Services.AddDbContext<GarageDoorsContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                options.EnableSensitiveDataLogging();
            });

            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<ILogService, LogService>();
            builder.Services.AddScoped<IDoorService, DoorService>();
            builder.Services.AddScoped<IUserDoorService, UserDoorService>();
            builder.Services.AddSingleton(new AuthService(
                jwtSettings.GetValue<string>("Secret"),
                jwtSettings.GetValue<string>("Issuer"),
                jwtSettings.GetValue<string>("Audience")
            ));

            builder.Services.AddScoped<IUserDoorRepository, UserDoorRepository>();
            builder.Services.AddScoped<IDoorRepository, DoorRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<ILogRepository, LogRepository>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            // JWT Middleware should be before Authentication and Authorization middleware
            app.UseMiddleware<JwtMiddleware>();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapDefaultControllerRoute();
            app.MapRazorPages();
            
            app.Run("http://0.0.0.0:5000");
        }
    }
}

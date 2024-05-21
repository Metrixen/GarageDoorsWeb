using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GarageDoorsWeb
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly ILogger<JwtMiddleware> _logger;

        public JwtMiddleware(RequestDelegate next, IConfiguration configuration, ILogger<JwtMiddleware> logger)
        {
            _next = next;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        
        
        {
            Console.WriteLine($"Processing request for {context.Request.Path}");
            var token = context.Request.Cookies["jwt"];
            if (token != null && !context.Request.Path.StartsWithSegments("/login"))
            {
                Console.WriteLine("JWT token found in cookies: {0}", token);
                AttachUserToContext(context, token);
            }
            else
            {
                Console.WriteLine("No JWT token found in cookies.");
            }

            if (context.Items.ContainsKey("User"))
            {
                Console.WriteLine("User authenticated: {0}", context.Items["User"]);
            }
            else
            {
                Console.WriteLine("User not authenticated.");
            }

            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]);

                Console.WriteLine("Validating token...");
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                Console.WriteLine("Token claims:");
                foreach (var claim in jwtToken.Claims)
                {
                    Console.WriteLine("{0}: {1}", claim.Type, claim.Value);
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, jwtToken.Claims.FirstOrDefault(x => x.Type == "name")?.Value),
                    new Claim(ClaimTypes.Role, jwtToken.Claims.FirstOrDefault(x => x.Type == "role")?.Value)
                };

                // Set the ClaimsIdentity
                var identity = new ClaimsIdentity(claims, JwtBearerDefaults.AuthenticationScheme);
                var claimsPrincipal = new ClaimsPrincipal(identity);

                context.User = claimsPrincipal;
                Console.WriteLine("Token validated successfully. User: {0}", context.User.Identity.Name);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Token validation failed. Exception: {0}", ex.Message);
                // Do nothing if JWT validation fails
                // The user will not be attached to context and the request will be unauthenticated
            }
        }

    }
}

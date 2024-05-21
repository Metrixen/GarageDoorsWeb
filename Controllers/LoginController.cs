using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageDoorsWeb.Controllers
{
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private readonly IUserService _userService;
        private readonly AuthService _authService;

        public LoginController(IUserService userService, AuthService authService)
        {
            _userService = userService;
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Index([FromForm] string username, [FromForm] string password)
        {
            var user = _userService.ValidateUser(username, password);
            if (user == null)
            {
                return Unauthorized("Invalid username or password.");
            }

            var token = _authService.GenerateJwtToken(user.Username, user.isAdmin, user.isOwner);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true, // Ensure the cookie is sent only over HTTPS
                SameSite = SameSiteMode.Strict, // Ensure the cookie is only sent in first-party contexts
                Expires = DateTime.UtcNow.AddDays(1) // Match the token expiration time
            };

            HttpContext.Response.Cookies.Append("jwt", token, cookieOptions);

            return Ok(new { redirectUrl = Url.Action("Index", "Home") });
        }

        [HttpPost("logout")]
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("jwt");
            return RedirectToPage("/Index");
        }
    }
}

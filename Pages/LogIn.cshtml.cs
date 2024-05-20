using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GarageDoorsWeb.Pages
{
    public class LoginModel : PageModel
    {
        private readonly AuthService _authService;

        public LoginModel(AuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string ErrorMessage { get; set; }

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Validate username and password (this is just a demo, you should use a user service)
            if (Username == "admin" && Password == "password")
            {
                var token = _authService.GenerateJwtToken(Username, isAdmin: true);
                HttpContext.Response.Cookies.Append("jwt", token, new CookieOptions { HttpOnly = true, Secure = true });
                return new JsonResult(new { token });
            }

            ErrorMessage = "Invalid username or password.";
            return Page();
        }
    }
}

using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GarageDoorsWeb.Pages
{
    public class AdminModel : PageModel
    {
        private readonly IUserService _userService;
        
        [BindProperty]
        public User NewUser { get; set; }
        public IEnumerable<User> Users { get; set; }


        public AdminModel(IUserService userService)
        {
            _userService = userService;
        }
        public void OnGet()
        {
            Users = _userService.GetAllUsers();
        }

        [HttpPost]
        public IActionResult OnPostAddUser()
        {
            if (!ModelState.IsValid)
            {
                // If the model state is not valid, redisplay the form with validation errors
                return Page();
            }

            // Convert the UserModel to a User entity
            var user = new User
            {
                // Assign properties from the UserModel
                Username = NewUser.Username,
                Password = NewUser.Password,
                isAdmin = NewUser.isAdmin
            };

            // Add the new user to the database via the service
            _userService.CreateUser(user);

            // Redirect to a different page or return a success message
            return RedirectToPage("/Admin");
        }
    }
}

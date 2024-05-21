using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageDoorsWeb.Controllers
{
    public class UserCreateController : Controller
    {
        private readonly IUserService _userService;

        public UserCreateController(IUserService userService)
        {
            _userService = userService;
        }

        public IActionResult Index()
        {
            // Retrieve list of users from the service
            var users = _userService.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult CreateUser()
        {
            // Return the view for creating a new user
            return View();
        }

        [HttpPost]
        public IActionResult CreateUser(User newUser)
        {
            if (ModelState.IsValid)
            {
                // Convert the UserModel to a User entity
                var user = new User
                {
                    // Assign properties from the UserModel
                    Username = newUser.Username,
                    Password = newUser.Password,
                    isAdmin = newUser.isAdmin,
                    isOwner = newUser.isOwner,
                };
                var currentUser = _userService.GetUserByUsername(User.Identity.Name);
                var createdByUserId = currentUser?.UserID;
                // Add the new user to the database via the service
                _userService.CreateUser(user, createdByUserId);

                // Redirect to the Index action to show the updated list of users
                return RedirectToAction("Index");
            }
            else
            {
                // If the model state is not valid, return the view with validation errors
                return View(newUser);
            }
        }
    }
}

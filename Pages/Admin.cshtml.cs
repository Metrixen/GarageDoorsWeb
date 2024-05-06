using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GarageDoorsWeb.Pages
{
    public class AdminModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IDoorService _doorService;

        [BindProperty]
        public User NewUser { get; set; }
        public IEnumerable<User> Users { get; set; }
        public List<Door> Doors { get; set; }

        public AdminModel(IUserService userService, IDoorService doorService)
        {
            _userService = userService;
            _doorService = doorService;
        }
        public void OnGet()
        {
            Users = _userService.GetAllUsers();
            Doors = _doorService.GetAllDoors().ToList();
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
        public IActionResult OnPostRenameDoor(int id, string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                var door = _doorService.GetDoorById(id);
                if (door != null)
                {
                    door.DoorName = newName;
                    _doorService.UpdateDoor(door);
                }
                return RedirectToPage();
            }
            else
            {
                // Handle the error case or set an error message if the newName is invalid
                ModelState.AddModelError("", "The new door name cannot be empty.");
                return Page();
            }
        }
        public IActionResult OnPostRemoveDoor(int id)
        {
            _doorService.DeleteDoor(id);
            return RedirectToPage();
        }
    }
}

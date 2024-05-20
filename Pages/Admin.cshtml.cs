using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GarageDoorsWeb.Pages
{
    [Authorize]
    [Authorize(Roles = "Admin")]
    public class AdminModel : PageModel
    {
        private readonly IUserService _userService;
        private readonly IDoorService _doorService;
        private readonly IUserDoorService _userDoorService;

        [BindProperty]
        public User NewUser { get; set; }
        public IEnumerable<User> Users { get; set; }
        // Property to hold the submitted door name
        [BindProperty]
        public string DoorName { get; set; }
        public List<Door> Doors { get; set; }
        public List<UserDoor> UserDoor { get; set; }

        [BindProperty]
        public int SelectedUserId { get; set; }  // Bind these properties to dropdowns in the UI

        [BindProperty]
        public int SelectedDoorId { get; set; }

        public AdminModel(IUserService userService, IDoorService doorService, IUserDoorService userDoorService)
        {
            _userService = userService;
            _doorService = doorService;
            _userDoorService = userDoorService;
        }
        public void OnGet()
        {
            Users = _userService.GetAllUsers();
            Doors = _doorService.GetAllDoors().ToList();
            UserDoor = _userDoorService.GetAllUserDoors().ToList();
        }

        [HttpPost]
        public IActionResult OnPostAddUser()
        { 
            //if (!ModelState.IsValid)
            //{
            //    // If the model state is not valid, redisplay the form with validation errors //Not set Error
            //    return Page();
            //}

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
        // Handler for adding a new door
        public IActionResult OnPostAddDoor()
        {
            if (!string.IsNullOrEmpty(DoorName))
            {
                var newDoor = new Door { DoorName = DoorName , LastModified=DateTime.Now};
                _doorService.AddDoor(newDoor);
                return RedirectToPage("/Admin");
            }
            else
            {
                Doors = (List<Door>)(_doorService.GetAllDoors() ?? Enumerable.Empty<Door>()); // Initialize Doors
                return Page();
            }
        }
        public IActionResult OnPostRenameDoor(int id, string newName)
        {
            if (!string.IsNullOrWhiteSpace(newName))
            {
                var door = _doorService.GetDoorById(id);
                if (door != null)
                {
                    door.LastModified = DateTime.Now;
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

        [HttpPost]
        public IActionResult OnPostAssignUserToDoor()
        {
            try
            {
                _userDoorService.AddUserToDoor(SelectedUserId, SelectedDoorId);
                TempData["Message"] = "User successfully assigned to door.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }

        [HttpPost]
        public IActionResult OnPostUnassignUserFromDoor()
        {
            try
            {
                _userDoorService.RemoveUserFromDoor(SelectedUserId, SelectedDoorId);
                TempData["Message"] = "User successfully unassigned from door.";
                return RedirectToPage();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Error: {ex.Message}");
                return Page();
            }
        }
        public IActionResult OnGetProtectedData()
        {
            var data = new { Message = "This is protected data." };
            return new JsonResult(data);
        }
    }
}

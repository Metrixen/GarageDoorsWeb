using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Linq;

namespace GarageDoorsWeb.Pages
{
    [Authorize(Roles = "Admin,Owner")]
    public class ManageUsersModel : PageModel
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
        public IEnumerable<UserDoor> UserDoors { get; set; }
        public IEnumerable<Door> UserDoorsList { get; set; }
        public IEnumerable<UserDoor> UserAssignedDoors { get; set; }

        [BindProperty]
        public int SelectedUserId { get; set; }  // Bind these properties to dropdowns in the UI

        [BindProperty]
        public int SelectedDoorId { get; set; }
        public ManageUsersModel(IUserService userService, IUserDoorService userDoorService , IDoorService doorService)
        {
            _userService = userService;
            _userDoorService = userDoorService;
            _doorService = doorService;
        }

        public void OnGet()
        {
            var currentUser = _userService.GetUserByUsername(User.Identity.Name);
            Users = _userService.GetUsersCreatedBy(currentUser.UserID);
            Doors = _doorService.GetAllDoors().ToList();
            UserDoors = _userDoorService.GetAllUserDoors()
                   .Where(ud => ud.UserID == currentUser.UserID);
            UserDoorsList = _userDoorService.GetDoorsByUserId(currentUser.UserID);
            UserAssignedDoors = _userDoorService.GetUserDoorsByOwnerId(currentUser.UserID);
        }

        public IActionResult OnPostAddUser()
        {
            // Convert the UserModel to a User entity
            var user = new User
            {
                // Assign properties from the UserModel
                Username = NewUser.Username,
                Password = NewUser.Password,
                isAdmin = NewUser.isAdmin,
                isOwner = NewUser.isOwner
            };

            // Get the current user's UserID
            var currentUser = _userService.GetUserByUsername(User.Identity.Name);
            var createdByUserId = currentUser?.UserID;

            // Add the new user to the database via the service
            _userService.CreateUser(user, createdByUserId);

            // Redirect to a different page or return a success message
            return RedirectToPage("/ManageUsers");
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
                    var currentUser = _userService.GetUserByUsername(User.Identity.Name);
                    int createdByUserId = (int)(currentUser?.UserID);
                    _doorService.UpdateDoor(door,createdByUserId);
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

        public IActionResult OnPostDeleteUser(int id)
        {
            _userService.DeleteUser(id);
            return RedirectToPage();
        }
    }
}

using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GarageDoorsWeb.Pages
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly IUserDoorService _userDoorService;
        private readonly IUserService _userService;
        private readonly IDoorService _doorService;
        private readonly INotificationService _notificationService;
        private readonly IHubContext<GarageDoorsWeb.Hubs.DoorStatusHub> _hubContext;

        public IndexModel(
            IUserService userService,
            IUserDoorService userDoorService,
            IDoorService doorService,
            INotificationService notificationService,
            IHubContext<GarageDoorsWeb.Hubs.DoorStatusHub> hubContext)
        {
            _userService = userService;
            _userDoorService = userDoorService;
            _doorService = doorService;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        // Property to hold the submitted door name
        [BindProperty]
        public string DoorName { get; set; }

        // Property to hold the list of doors to display
        public IEnumerable<Door> Doors { get; set; }

        public void OnGet()
        {
            var username = User.Identity.Name;
            var user = _userService.GetUserByUsername(username);
            Doors = _userDoorService.GetDoorsByUserId(user.UserID);
        }

       

        // Handler for toggling the status of a door
        public async Task<IActionResult> OnPostToggleDoor(int doorId)
        {
            try
            {
                var door = _doorService.GetDoorById(doorId);
                door.IsOpen = !door.IsOpen;
                var currentUser = _userService.GetUserByUsername(User.Identity.Name);
                int createdByUserId = (int)(currentUser?.UserID);
                _doorService.UpdateDoor(door, createdByUserId);

                // Notify users about door status change via the notification service
                var doorName = door.DoorName;
                var username = currentUser?.Username ?? "Unknown";
                if (door.IsOpen)
                {
                    await _notificationService.SendDoorOpenedAsync(doorName, username);
                }
                else
                {
                    await _notificationService.SendDoorClosedAsync(doorName, username);
                }
                // Broadcast the change through SignalR hub
                await _hubContext.Clients.All.SendAsync("ReceiveDoorStatus", doorId, door.IsOpen);

                return RedirectToPage("/Index");
            }
            catch (ArgumentException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
                return RedirectToPage("/Index");
            }
        }
    }
}
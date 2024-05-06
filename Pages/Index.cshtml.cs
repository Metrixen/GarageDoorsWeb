using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GarageDoorsWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IDoorService _doorService;

        public IndexModel(IDoorService doorService)
        {
            _doorService = doorService;
        }

        // Property to hold the submitted door name
        [BindProperty]
        public string DoorName { get; set; }

        // Property to hold the list of doors to display
        public IEnumerable<Door> Doors { get; set; }

        public void OnGet()
        {
            // Populate the list of doors
            Doors = _doorService.GetAllDoors() ?? Enumerable.Empty<Door>();
        }

        // Handler for adding a new door
        public IActionResult OnPostAddDoor()
        {
            if (!string.IsNullOrEmpty(DoorName))
            {
                var newDoor = new Door { DoorName = DoorName };
                _doorService.AddDoor(newDoor);
                return RedirectToPage("/Index");
            }
            else
            {
                Doors = _doorService.GetAllDoors() ?? Enumerable.Empty<Door>(); // Initialize Doors
                return Page();
            }
        }

        // Handler for toggling the status of a door
        public IActionResult OnPostToggleDoor(int doorId)
        {
            try
            {
                var door = _doorService.GetDoorById(doorId);
                door.IsOpen = !door.IsOpen;
                _doorService.UpdateDoor(door);
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
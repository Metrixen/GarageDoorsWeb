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
            Doors = _doorService.GetAllDoors();
        }

        // Handler for adding a new door
        public IActionResult OnPostAddDoor()
        {
            if (!string.IsNullOrEmpty(DoorName))
            {
                // Create a new door object
                var newDoor = new Door
                {
                    DoorName = DoorName,
                    // Set other properties of the door as needed
                };

                // Add the new door to the database
                _doorService.AddDoor(newDoor);

                // Redirect back to the index page
                return RedirectToPage("/Index");
            }
            else
            {
                // Handle invalid input, such as displaying an error message
                return Page();
            }
        }

        // Handler for toggling the status of a door
        public IActionResult OnPostToggleDoor(int doorId)
        {
            var door = _doorService.GetDoorById(doorId);
            if (door != null)
            {
                // Door found, proceed with toggling logic
                // For example:
                door.IsOpen = !door.IsOpen; // Toggle the IsOpen status
                _doorService.UpdateDoor(door); // Update the door in the database
                return RedirectToPage("/Index"); // Redirect back to the index page
            }
            else
            {
                // Handle case where door with specified ID was not found
                // For example:
                TempData["ErrorMessage"] = "Door not found."; // Store error message in TempData
                return RedirectToPage("/Index"); // Redirect back to the index page
            }
        }
    }
}
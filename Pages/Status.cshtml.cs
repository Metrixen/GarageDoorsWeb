using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Pages
{
    [Authorize]
    public class StatusModel : PageModel
    {
        private readonly GarageDoorsContext _context;
        private readonly IUserService _userService;

        public StatusModel(GarageDoorsContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            // Get the current user based on username
            var currentUser = _userService.GetUserByUsername(User.Identity.Name);
            if (currentUser == null)
            {
                return NotFound("User not found");
            }

            // Retrieve only the doors assigned to the current user
            var userDoors = await _context.UserDoors
                                          .Where(d => d.UserID == currentUser.UserID) // Assuming there's a UserID property in the Doors table
                                          .Select(d => new
                                          {
                                              d.DoorID,
                                              d.Door.DoorName,
                                              d.Door.IsOpen,
                                              d.Door.IsLocked
                                          })
                                          .ToListAsync();

            return new JsonResult(userDoors);
        }
    }
}

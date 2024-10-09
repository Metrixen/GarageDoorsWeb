using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace GarageDoorsWeb.Controllers
{
    public class DoorCreateController : Controller
    {
        private readonly IDoorService _doorService;
        private readonly IUserDoorService _userDoorService;
        private readonly IUserService _userService;

        public DoorCreateController(IDoorService doorService, IUserService userService)
        {
            _doorService = doorService;
            _userService = userService;
        }

        // GET: Door
        public IActionResult Index()
        {
            var doors = _doorService.GetAllDoors();
            return View(doors);
        }

        // GET: Door/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Door/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Door door)
        {
            if (ModelState.IsValid)
            {
                _doorService.AddDoor(door);
                return RedirectToAction(nameof(Index));
            }
            return View(door);
        }

        // GET: Door/Edit/5
        public IActionResult Edit(int id)
        {
            var door = _doorService.GetDoorById(id);
            if (door == null)
            {
                return NotFound();
            }
            return View(door);
        }

        // POST: Door/Toggle/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int id)
        {
            var door = _doorService.GetDoorById(id);
            if (door == null)
            {
                return NotFound();
            }
            
            // Toggle the IsOpen property
            door.IsOpen = !door.IsOpen;
            int userId = _userService.GetUserByUsername(User.Identity.Name)?.UserID ?? 0;

            if (userId == 0)
            {
                return BadRequest("User not found");
            }
            _doorService.UpdateDoor(door,userId);
            return RedirectToAction(nameof(Index));
        }

        // POST: Door/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, Door door)
        {
            if (id != door.DoorID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                int userId = _userService.GetUserByUsername(User.Identity.Name)?.UserID ?? 0;
                _doorService.UpdateDoor(door,userId);
                return RedirectToAction(nameof(Index));
            }
            return View(door);
        }

        // GET: Door/Delete/5
        public IActionResult Delete(int id)
        {
            var door = _doorService.GetDoorById(id);
            if (door == null)
            {
                return NotFound();
            }
            return View(door);
        }

        // POST: Door/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            _doorService.DeleteDoor(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult AssignUserToDoor(int userId, int doorId)
        {
            try
            {
                _userDoorService.AddUserToDoor(userId, doorId);
                return Ok("User assigned to door successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}

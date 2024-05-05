using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GarageDoorsWeb.Controllers
{
    public class DoorCreateController : Controller
    {
        private readonly IDoorService _doorService;

        public DoorCreateController(IDoorService doorService)
        {
            _doorService = doorService;
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

            _doorService.UpdateDoor(door);
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
                _doorService.UpdateDoor(door);
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
    }
}

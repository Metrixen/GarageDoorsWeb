using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Controllers
{
    [Route("api/door")]
    [ApiController]
    public class DoorController : ControllerBase
    {
        private readonly IDoorService _doorService;

        public DoorController(IDoorService doorService)
        {
            _doorService = doorService;
        }

        // GET: api/Door
        [HttpGet]
        public ActionResult<IEnumerable<Door>> GetAllDoors()
        {
            return Ok(_doorService.GetAllDoors());
        }

        // GET: api/Door/{id}
        [HttpGet("{id}")]
        public ActionResult<Door> GetDoor(int id)
        {
            var door = _doorService.GetDoorById(id);
            if (door == null)
            {
                return NotFound("Door not found.");
            }
            return Ok(door);
        }

        // POST: api/Door
        [HttpPost]
        public IActionResult AddDoor([FromBody] Door door)
        {
            if (door == null)
            {
                return BadRequest("Invalid door data.");
            }
            _doorService.AddDoor(door);
            return CreatedAtAction(nameof(GetDoor), new { id = door.DoorID }, door);
        }

        // PUT: api/Door/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateDoor(int id, [FromBody] Door door)
        {
            if (door == null || id != door.DoorID)
            {
                return BadRequest("Invalid door data or mismatched ID.");
            }

            var existingDoor = _doorService.GetDoorById(id);
            if (existingDoor == null)
            {
                return NotFound("Door not found.");
            }

            _doorService.UpdateDoor(door);
            return NoContent();
        }

        // DELETE: api/Door/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteDoor(int id)
        {
            var door = _doorService.GetDoorById(id);
            if (door == null)
            {
                return NotFound("Door not found.");
            }
            _doorService.DeleteDoor(id);
            return NoContent();
        }

        // PUT: api/Door/{id}/toggle
        [HttpPut("{id}/toggle")]
        public IActionResult ToggleDoor(int id, [FromQuery] int userId, [FromQuery] bool isOpen)
        {
            var door = _doorService.GetDoorById(id);
            if (door == null)
            {
                return NotFound("Door not found.");
            }

            _doorService.ToggleDoor(id, userId, isOpen);
            return NoContent();
        }

        // PUT: api/Door/{id}/lock
       /* [HttpPut("{id}/lock")]
        public IActionResult LockOrUnlockDoor(int id, [FromQuery] int userId, [FromQuery] bool isLocked)
        {
            var door = _doorService.GetDoorById(id);
            if (door == null)
            {
                return NotFound("Door not found.");
            }

            _doorService.LockOrUnlockDoor(id, userId, isLocked);
            return NoContent();
        }*/
    }
}

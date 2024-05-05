using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        // POST: api/Log
        [HttpPost]
        public IActionResult LogUserAction([FromBody] LogEntry logEntry)
        {
            if (logEntry == null || string.IsNullOrEmpty(logEntry.Action))
            {
                return BadRequest("Invalid log entry data.");
            }

            _logService.LogUserAction(logEntry.UserId, logEntry.DoorId, logEntry.Action);
            return NoContent(); // Logging does not typically return content
        }

        // GET: api/Log/User/5
        [HttpGet("User/{userId}")]
        public ActionResult<IEnumerable<Logs>> GetLogsByUser(int userId)
        {
            var logs = _logService.GetLogsByUser(userId);
            if (logs == null || !logs.Any())
            {
                return NotFound("No logs found for the given user.");
            }
            return Ok(logs);
        }

        // GET: api/Log/Door/5
        [HttpGet("Door/{doorId}")]
        public ActionResult<IEnumerable<Logs>> GetLogsByDoor(int doorId)
        {
            var logs = _logService.GetLogsByDoor(doorId);
            if (logs == null || !logs.Any())
            {
                return NotFound("No logs found for the given door.");
            }
            return Ok(logs);
        }
    }
    public class LogEntry
    {
        public int UserId { get; set; }
        public int DoorId { get; set; }
        public string Action { get; set; } // "Opened", "Closed", "Locked", "Unlocked"
    }
}

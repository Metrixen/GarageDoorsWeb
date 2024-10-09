using GarageDoorsWeb.Models;
using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GarageDoorsWeb.Pages
{
    [Authorize(Roles = "Admin")]
    public class LogsModel : PageModel
    {
        private readonly ILogService _logService;
        private readonly IUserService _userService;
        private readonly IDoorService _doorService;

        public LogsModel(ILogService logService, IUserService userService, IDoorService doorService)
        {
            _logService = logService;
            _userService = userService;
            _doorService = doorService;
        }

        // Property to bind the selected date
        [BindProperty(SupportsGet = true)]
        public DateTime? SelectedDate { get; set; }

        // List of logs to display
        public IEnumerable<LogViewModel> Logs { get; set; }

        public void OnGet()
        {
            // Retrieve logs for the selected date, if provided
            var logs = SelectedDate.HasValue
                ? _logService.GetLogsByDate(SelectedDate.Value)
                : new List<Logs>();

            // Map logs to the LogViewModel, including user and door information
            Logs = logs.Select(log => new LogViewModel
            {
                LogID = log.LogID,
                Username = _userService.GetUserById(log.UserID)?.Username ?? "Unknown",
                DoorName = _doorService.GetDoorById(log.DoorID)?.DoorName ?? "Unknown",
                Action = log.Action,
                Date = log.Date
            }).ToList();
        }
    }

    public class LogViewModel
    {
        public int LogID { get; set; }
        public string Username { get; set; }
        public string DoorName { get; set; }
        public string Action { get; set; }
        public DateTime Date { get; set; }
    }
}

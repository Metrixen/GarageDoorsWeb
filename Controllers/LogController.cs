using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageDoorsWeb.Controllers
{
    public class LogController : Controller
    {
        private readonly ILogService _logService;

        public LogController(ILogService logService)
        {
            _logService = logService;
        }

        public IActionResult LogsByDate(DateTime date)
        {
            var logs = _logService.GetLogsByDate(date);
            return View(logs);
        }
    }
}

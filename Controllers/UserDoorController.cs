using GarageDoorsWeb.Services;
using Microsoft.AspNetCore.Mvc;

namespace GarageDoorsWeb.Controllers
{
    public class UserDoorController : ControllerBase
    {
        private readonly IUserDoorService _userDoorService;

        public UserDoorController(IUserDoorService userDoorService)
        {
            _userDoorService = userDoorService;
        }

        //[HttpPost]
        //public IActionResult AssignUserToDoor(int userId, int doorId)
        //{
        //    try
        //    {
        //        _userDoorService.AddUserToDoor(userId, doorId);
        //        return Ok("User assigned to door successfully.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}
    }
}

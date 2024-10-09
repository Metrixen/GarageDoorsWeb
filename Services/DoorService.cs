using GarageDoorsWeb.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Services
{
    public class DoorService:IDoorService
    {
        private readonly GarageDoorsContext _dbcontext;
        private readonly ILogService _logService;

        public DoorService(GarageDoorsContext context, ILogService logService)
        {
            _dbcontext = context;
            _logService = logService;
        }
        public void AddDoor(Door door)
        {
            _dbcontext.Doors.Add(door);
            _dbcontext.SaveChanges();
        }
        public IEnumerable<Door> GetAllDoors()
        {
            try
            {
                return _dbcontext.Doors.ToList();
            }
            catch (Exception ex)
            {
                // Log the exception (use your logging framework of choice)
                Console.WriteLine(ex.Message); // Placeholder for actual logging

                // Return an empty list on error to prevent null references elsewhere
                return Enumerable.Empty<Door>();
            }
        }
        public Door GetDoorById(int doorId)
        {
            var door = _dbcontext.Doors.Find(doorId);
            if (door == null)
            {
                throw new ArgumentException("Door not found.");
            }
            return door;
        }
        public IEnumerable<Door> GetDoorsByUserId(int userid)
        {
            var userDoors = _dbcontext.UserDoors
                .Where(ud => ud.UserID == userid)
                .Include(ud => ud.Door)
                .Select(ud => ud.Door)
                .ToList();

            return userDoors;
        }
        public void UpdateDoor(Door door, int userID)
        {
            _dbcontext.Doors.Update(door);
            _dbcontext.SaveChanges();


            // Log the action
            string action = door.IsOpen ? "Opened" : "Closed";
            string description = $"Door {door.DoorID} was {action.ToLower()} by user {userID}";

            _logService.LogUserAction(userID, door.DoorID, action);
        }

        public void DeleteDoor(int doorId)
        {
            var door = _dbcontext.Doors.Find(doorId);
            if (door != null)
            {
                _dbcontext.Doors.Remove(door);
                _dbcontext.SaveChanges();
            }
        }
        public void ToggleDoor(int doorId, int userId, bool isOpen)
        {
            var door = _dbcontext.Doors.Find(doorId);
            if (door != null)
            {
                door.IsOpen = isOpen;
                _dbcontext.SaveChanges();

                // Log the action
                string action = isOpen ? "Opened" : "Closed";

                _logService.LogUserAction(userId, doorId, action);
            }
        }
    }
}

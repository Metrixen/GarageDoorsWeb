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

        public DoorService(GarageDoorsContext context)
        {
            _dbcontext = context;
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
        public void UpdateDoor(Door door)
        {
            _dbcontext.Doors.Update(door);
            _dbcontext.SaveChanges();
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
                var log = new Logs
                {
                    UserID = userId,
                    DoorID = doorId,
                    Date = DateTime.Now,
                    Action = isOpen ? "Opened" : "Closed"
                };
                _dbcontext.Logs.Add(log);
                _dbcontext.SaveChanges();
            }
        }
    }
}

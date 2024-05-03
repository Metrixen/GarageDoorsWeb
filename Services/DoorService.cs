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
            return _dbcontext.Doors.ToList();
        }
        public Door GetDoorById(int doorId)
        {
            return _dbcontext.Doors.Find(doorId);
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

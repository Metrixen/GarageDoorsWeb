using GarageDoorsWeb.Models;
using GarageDoorsWeb.Repositories.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Repositories
{
    public class DoorRepository : IDoorRepository
    {
        private readonly GarageDoorsContext _dbcontext;

        public DoorRepository(GarageDoorsContext context)
        {
            _dbcontext = context;
        }

        public Door GetDoorById(int doorId)
        {
            return _dbcontext.Doors.Find(doorId);
        }

        public IEnumerable<Door> GetAllDoors()
        {
            return _dbcontext.Doors.ToList();
        }

        public void AddDoor(Door door)
        {
            _dbcontext.Doors.Add(door);
            _dbcontext.SaveChanges();
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

        public IEnumerable<Door> GetDoorsByStatus(bool isOpen)
        {
            return _dbcontext.Doors.Where(d => d.IsOpen == isOpen).ToList();
        }
    }

}

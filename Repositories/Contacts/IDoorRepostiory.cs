using GarageDoorsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Repositories.Contacts
{
    public interface IDoorRepository
    {
        Door GetDoorById(int doorId);
        IEnumerable<Door> GetAllDoors();
        void AddDoor(Door door);
        void UpdateDoor(Door door);
        void DeleteDoor(int doorId);
        IEnumerable<Door> GetDoorsByStatus(bool isOpen);
    }
}

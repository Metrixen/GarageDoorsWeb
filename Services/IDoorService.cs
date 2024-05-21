using GarageDoorsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Services
{
    public interface IDoorService
    {
        void AddDoor(Door door);
        IEnumerable<Door> GetAllDoors();
        Door GetDoorById(int doorId);
        IEnumerable<Door> GetDoorsByUserId(int userid);
        void UpdateDoor(Door door);
        void DeleteDoor(int doorId);
        void ToggleDoor(int doorId, int userId, bool isOpen);

    }
}

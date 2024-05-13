using GarageDoorsWeb.Models;

namespace GarageDoorsWeb.Services
{
    public interface IUserDoorService
    {
        IEnumerable<UserDoor> GetAllUserDoors();
        void AddUserToDoor(int userId, int doorId);
        void RemoveUserFromDoor(int userId, int doorId);
    }
}

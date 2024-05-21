using GarageDoorsWeb.Models;

namespace GarageDoorsWeb.Services
{
    public interface IUserDoorService
    {
        IEnumerable<UserDoor> GetAllUserDoors();
        IEnumerable<Door> GetDoorsByUserId(int userId);
        IEnumerable<UserDoor> GetUserDoorsByOwnerId(int ownerId);
        void AddUserToDoor(int userId, int doorId);
        void RemoveUserFromDoor(int userId, int doorId);
    }
}

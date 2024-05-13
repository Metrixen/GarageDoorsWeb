using GarageDoorsWeb.Models;

namespace GarageDoorsWeb.Repositories.Contacts
{
    public interface IUserDoorRepository
    {
        void AddUserToDoor(UserDoor userDoor);
        UserDoor FindUserDoor(int userId, int doorId);
    }
}

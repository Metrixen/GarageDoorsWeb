using GarageDoorsWeb.Models;
using GarageDoorsWeb.Repositories.Contacts;

namespace GarageDoorsWeb.Repositories
{

    public class UserDoorRepository : IUserDoorRepository
    {
        private readonly GarageDoorsContext _context;

        public UserDoorRepository(GarageDoorsContext context)
        {
            _context = context;
        }

        public void AddUserToDoor(UserDoor userDoor)
        {
            _context.UserDoors.Add(userDoor);
            _context.SaveChanges();
        }

        public UserDoor FindUserDoor(int userId, int doorId)
        {
            return _context.UserDoors.FirstOrDefault(ud => ud.UserID == userId && ud.DoorID == doorId);
        }
    }
}

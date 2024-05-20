using GarageDoorsWeb.Models;
using GarageDoorsWeb.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;

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
        public IEnumerable<UserDoor> GetAllUserDoors()
        {
            return _context.UserDoors
                           .Include(ud => ud.User) // Include related User
                           .Include(ud => ud.Door) // Include related Door
                           .ToList();
        }
    }
}

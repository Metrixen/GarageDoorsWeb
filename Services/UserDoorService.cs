using GarageDoorsWeb.Models;
using GarageDoorsWeb.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;

namespace GarageDoorsWeb.Services
{
    public class UserDoorService : IUserDoorService
    {
        private readonly IUserRepository _userRepository;
        private readonly IDoorRepository _doorRepository;
        private readonly IUserDoorRepository _userDoorRepository;
        private GarageDoorsContext _dbcontext;

        public UserDoorService(IUserRepository userRepository, IDoorRepository doorRepository, IUserDoorRepository userDoorRepository, GarageDoorsContext context)
        {
            _userRepository = userRepository;
            _doorRepository = doorRepository;
            _userDoorRepository = userDoorRepository;
            _dbcontext = context;
        }
        public IEnumerable<UserDoor> GetAllUserDoors()
        {
            try
            {
                return _dbcontext.UserDoors.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); // Placeholder for actual logging

                // Return an empty list on error to prevent null references elsewhere
                return Enumerable.Empty<UserDoor>();
            }
        }
        public void AddUserToDoor(int userId, int doorId)
        {
            // Check if the User exists
            var user = _userRepository.GetUserById(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            // Check if the Door exists
            var door = _doorRepository.GetDoorById(doorId);
            if (door == null)
                throw new ArgumentException("Door not found");

            // Check if the UserDoor relationship already exists
            var existingLink = _userDoorRepository.FindUserDoor(userId, doorId);
            if (existingLink != null)
                throw new InvalidOperationException("This user is already linked to this door");

            // Create a new UserDoor entry
            var userDoor = new UserDoor
            {
                UserID = userId,
                DoorID = doorId
            };

            _userDoorRepository.AddUserToDoor(userDoor);
        }
        public void RemoveUserFromDoor(int userId, int doorId)
        {
            var userDoor = _dbcontext.UserDoors.FirstOrDefault(ud => ud.UserID == userId && ud.DoorID == doorId);
            if (userDoor != null)
            {
                _dbcontext.UserDoors.Remove(userDoor);
                _dbcontext.SaveChanges();
            }
            else
            {
                throw new InvalidOperationException("Assignment not found.");
            }
        }
    }
}

using GarageDoorsWeb.Models;
using GarageDoorsWeb.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly GarageDoorsContext _context;

        public UserService(IUserRepository userRepository, GarageDoorsContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public User GetUserById(int userId)
        {
            return _userRepository.GetUserById(userId);
        }
        public User GetUserByUsername(string username)
        {
            return _userRepository.GetUserByUsername(username);
        }
        public IEnumerable<User> GetAllUsers()
        {
            return _userRepository.GetAllUsers();
        }
        public User ValidateUser(string username, string password)
        {
            var user = _userRepository.GetAllUsers()
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            return user;
        }
        public void CreateUser(User user, int? createdByUserId)
        {
            user.CreatedBy = createdByUserId;
            _userRepository.AddUser(user);
        }

        public void UpdateUser(User user)
        {
            _userRepository.UpdateUser(user);
        }
        public IEnumerable<User> GetUsersCreatedBy(int creatorUserId)
        {
            return _context.Users.Where(u => u.CreatedBy == creatorUserId).ToList();
        }
        public void DeleteUser(int userId)
        {
            _userRepository.DeleteUser(userId);
        }
    }

}

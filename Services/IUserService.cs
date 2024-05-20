using GarageDoorsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Services
{
    public interface IUserService
    {
        User GetUserById(int userId);
        User GetUserByUsername(string username);
        IEnumerable<User> GetAllUsers();
        User ValidateUser(string username, string password);
        void CreateUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int userId);
    }
}

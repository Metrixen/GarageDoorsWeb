using GarageDoorsWeb.Models;
using GarageDoorsWeb.Repositories.Contacts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly GarageDoorsContext _dbcontext;

        public UserRepository(GarageDoorsContext context)
        {
            _dbcontext = context;
        }

        public User GetUserById(int userId)
        {
            return _dbcontext.Users.Find(userId);
        }
        public User GetUserByUsername(string username)
        {
            return _dbcontext.Users.FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<User> GetAllUsers()
        {
            return _dbcontext.Users.ToList();
        }

        public void AddUser(User user)
        {
            _dbcontext.Users.Add(user);
            _dbcontext.SaveChanges();
        }

        public void UpdateUser(User user)
        {
            _dbcontext.Entry(user).State = EntityState.Modified;
            _dbcontext.SaveChanges();
        }

        public void DeleteUser(int userId)
        {
            var user = _dbcontext.Users.Find(userId);
            if (user != null)
            {
                _dbcontext.Users.Remove(user);
                _dbcontext.SaveChanges();
            }
        }
    }
}

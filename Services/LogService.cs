using GarageDoorsWeb.Models;
using GarageDoorsWeb.Repositories.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Services
{
    public class LogService : ILogService
    {
        private readonly ILogRepository _logRepository;
        private readonly IUserRepository _userRepository; // If needed to validate user existence

        public LogService(ILogRepository logRepository, IUserRepository userRepository)
        {
            _logRepository = logRepository;
            _userRepository = userRepository;
        }

        public void LogUserAction(int userId, int doorId, string action)
        {
            var log = new Logs
            {
                UserID = userId,
                DoorID = doorId,
                Date = DateTime.Now,
                Action = action
            };
            _logRepository.AddLog(log);
        }

        public IEnumerable<Logs> GetLogsByUser(int userId)
        {
            return _logRepository.GetLogsByUserId(userId);
        }

        public IEnumerable<Logs> GetLogsByDoor(int doorId)
        {
            return _logRepository.GetLogsByDoorId(doorId);
        }

        public IEnumerable<Logs> GetLogsByDate(DateTime date)
        {
            return _logRepository.GetLogsByDate(date);
        }
    }
}

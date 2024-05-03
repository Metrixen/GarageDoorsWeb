using GarageDoorsWeb.Models;
using GarageDoorsWeb.Repositories.Contacts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Repositories
{
    public class LogRepository : ILogRepository
    {
        private readonly GarageDoorsContext _context;

        public LogRepository(GarageDoorsContext context)
        {
            _context = context;
        }

        public void AddLog(Logs log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public IEnumerable<Logs> GetLogsByUserId(int userId)
        {
            return _context.Logs.Where(log => log.UserID == userId).ToList();
        }

        public IEnumerable<Logs> GetLogsByDoorId(int doorId)
        {
            return _context.Logs.Where(log => log.DoorID == doorId).ToList();
        }
    }

}

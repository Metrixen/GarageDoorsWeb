using GarageDoorsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Services
{
    public interface ILogService
    {
        void LogUserAction(int userId, int doorId, string action);
        IEnumerable<Logs> GetLogsByUser(int userId);
        IEnumerable<Logs> GetLogsByDoor(int doorId);
    }
}

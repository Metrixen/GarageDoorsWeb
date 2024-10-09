using GarageDoorsWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Repositories.Contacts
{
    public interface ILogRepository
    {
        void AddLog(Logs log);
        IEnumerable<Logs> GetLogsByUserId(int userId);
        IEnumerable<Logs> GetLogsByDoorId(int doorId);
        IEnumerable<Logs> GetLogsByDate(DateTime date);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GarageDoorsWeb.Models;
using Microsoft.EntityFrameworkCore;

namespace GarageDoorsWeb
{
    public class GarageDoorsContext : DbContext
    {
        public GarageDoorsContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<Door> Doors { get; set; }
    }
}

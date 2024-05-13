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
        public DbSet<UserDoor> UserDoors { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the composite key for UserDoor
            modelBuilder.Entity<UserDoor>()
                .HasKey(ud => new { ud.UserID, ud.DoorID });

            // Optional: Configure relationships if not already implicitly configured
            modelBuilder.Entity<UserDoor>()
                .HasOne(ud => ud.User)
                .WithMany(u => u.UserDoors)
                .HasForeignKey(ud => ud.UserID);

            modelBuilder.Entity<UserDoor>()
                .HasOne(ud => ud.Door)
                .WithMany(d => d.UserDoors)
                .HasForeignKey(ud => ud.DoorID);
        }

    }
}

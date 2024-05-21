using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GarageDoorsWeb.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }  // Consider storing hashed passwords for security
        public bool isOnline { get; set; }
        public bool isBlocked { get; set; }
        public bool isAdmin { get; set; }
        public bool isOwner { get; set; }
        public int? CreatedBy { get; set; }

        // Navigation property to link Logs
        public virtual ICollection<Logs> Logs { get; set; } = new HashSet<Logs>();
        public virtual ICollection<UserDoor> UserDoors { get; set; } = new HashSet<UserDoor>();
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Models
{
    public class Door
    {
        [Key]
        public int DoorID { get; set; }
        public string DoorName { get; set; }
        public bool IsOpen { get; set; }
        public bool IsLocked { get; set; }
        public DateTime LastModified { get; set; }

        public virtual ICollection<Logs> Logs { get; set; } = new HashSet<Logs>();
        public virtual ICollection<UserDoor> UserDoors { get; set; } = new HashSet<UserDoor>();

    }
}

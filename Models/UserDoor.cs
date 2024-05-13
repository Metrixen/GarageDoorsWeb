using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GarageDoorsWeb.Models
{
    public class UserDoor
    {
        [ForeignKey("User")]
        public int UserID { get; set; } // Foreign Key
        public virtual User User { get; set; }

        [ForeignKey("Door")]
        public int DoorID { get; set; } // Foreign Key
        public virtual Door Door { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GarageDoorsWeb.Models
{
    public class Logs
    {
        [Key]
        public int LogID { get; set; }
        [ForeignKey("User")]
        public int UserID { get; set; }
        public virtual User User { get; set; }
        public DateTime Date { get; set; }

        // Adding door information
        [ForeignKey("Door")]
        public int DoorID { get; set; }
        public virtual Door Door { get; set; }
        public string Action { get; set; } // "Opened" or "Closed"
    }
}

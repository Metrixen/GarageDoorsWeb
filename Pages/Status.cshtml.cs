using GarageDoorsWeb.Models;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace GarageDoorsWeb.Pages
{
    public class StatusModel : PageModel
    {
        private readonly GarageDoorsContext _context;

        public StatusModel(GarageDoorsContext context)
        {
            _context = context;
        }

        public string Status { get; set; }
        public List<Door> Doors { get; set; } // Changed from IEnumerable to List for simplicity

        public async Task OnGetAsync()
        {
            Doors = await _context.Doors.ToListAsync();
        }
    }
}

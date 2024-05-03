using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace GarageDoorsWeb.Pages
{
    public class StatusModel : PageModel
    {
        public string Status { get; set; }
        public void OnPost(string status)
        {
            Status = status;
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainService.Pages.razorPages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class UserAdminModel : PageModel
    {
        [BindProperty]
        public string message { get; set; }
        public void OnGet()
        {
            message = HttpContext.Session.GetString("SelectedUser");
        }
    }
}

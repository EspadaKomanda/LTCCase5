using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainService.Pages.razorPages
{
    [Authorize]
    public class AnketModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}

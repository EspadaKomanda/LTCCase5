using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MainService.Pages.razorPages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        public void OnPost(string email, string password)
        {
            Console.WriteLine(email + ":" + password);
        }
        public void OnGet()
        {

        }
    }
}

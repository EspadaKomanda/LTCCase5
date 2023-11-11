using System.Security.Claims;
using MainService.Controllers.Models;
using Microsoft.AspNetCore.Mvc;

namespace MainService.Controllers
{
    [ApiController]
    [Route("/users")]
    public class LogicController:ControllerBase
    {
        [Route("selectUser")]
        public async Task<IActionResult> SelectUser([FromQuery] UserSelectModel model)
        {
            HttpContext.Session.SetString("SelectedUser",model.userId);
            return RedirectToPage("/UserAdmin");
        }
    }
}

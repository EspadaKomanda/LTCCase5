using MainService.Communicators;
using MainService.Pages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MainService.Pages.razorPages
{
    [Authorize]
    public class ProfileModel : PageModel
    {
        private AuthDbCommunicator dbCommunicator = new AuthDbCommunicator();
        [BindProperty(SupportsGet = true)] 
        public UserModel userModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public string inviteLetter { get; set; }
        public ProfileModel()
        {
            
        }
        public void OnGet()
        {
            inviteLetter = "Пример пригласительного письма!";
            GetReply result = dbCommunicator.GetUserByEmail(HttpContext.User.FindFirst(ClaimTypes.Email).Value).Result;
            
            Guid currentUuid;
            Guid.TryParse(result.Uuid, out currentUuid);
            userModel = new UserModel()
            {
                about = result.About,
                avatar = result.Avatar,
                email = result.Email,
                firstName = result.FirstName,
                lastName = result.LastName,
                password = result.Password,
                patronymic = result.Patronymic,
                phone = result.Phone,
                position = result.Position,
                role = result.Role,
                telegram = result.Telegram,
                uuid = currentUuid,
                dateOfBirth = result.DateOfBirth
            };
        }
    }
}

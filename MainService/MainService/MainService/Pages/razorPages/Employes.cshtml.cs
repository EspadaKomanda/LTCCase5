using MainService.Communicators;
using MainService.Pages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.Ocsp;

namespace MainService.Pages.razorPages
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class EmployesModel : PageModel
    {
        private AuthDbCommunicator _communicator = new AuthDbCommunicator();
        [BindProperty]
        public List<UserModel> users { get; set; }
        public async Task OnGet()
        {
           var result = await _communicator.getUsers();
           users = await convertList(result.Users.ToList());

        }

        private async Task<List<UserModel>> convertList(List<GetReply> request)
        {

            List<UserModel> result = new List<UserModel>();
            foreach (var VARIABLE in request)
            {
                Guid currentUuid;
                Guid.TryParse(VARIABLE.Uuid, out currentUuid);
                var user = new UserModel()
                {
                    uuid = currentUuid,
                    email = VARIABLE.Email,
                    phone = VARIABLE.Phone,
                    telegram = VARIABLE.Telegram,
                    firstName = VARIABLE.FirstName,
                    lastName = VARIABLE.LastName,
                    password = VARIABLE.Password,
                    patronymic = VARIABLE.Patronymic,
                    position = VARIABLE.Position,
                    role = VARIABLE.Role,
                    about = VARIABLE.About,
                    avatar = VARIABLE.Avatar,
                    dateOfBirth = VARIABLE.DateOfBirth
                };
                result.Add(user);
            }
            return result;
        }
    }
}

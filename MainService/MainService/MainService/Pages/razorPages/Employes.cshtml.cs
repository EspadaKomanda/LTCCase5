using System.Security.Cryptography;
using MainService.Communicators;
using MainService.Pages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using System.Security.Claims;

namespace MainService.Pages.razorPages
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class EmployesModel : PageModel
    {
        private AuthDbCommunicator _communicator = new AuthDbCommunicator();
        private LoggerCommunicator _logger = new LoggerCommunicator();
        [BindProperty]
        public List<UserModel> users { get; set; }
        [BindProperty]
        public CreationUserModel model {get; set; }
        [BindProperty]
        public UserModel userModel { get; set; }
        public async Task OnGet()
        {
           var res = await _communicator.getUsers();
           users = await convertList(res.Users.ToList());
           GetReply result = _communicator.GetUserByEmail(HttpContext.User.FindFirst(ClaimTypes.Email).Value).Result;

           Guid currentUuid;
           Guid.TryParse(result.Uuid, out currentUuid);
           userModel = new UserModel()
           {
               about = result.About,
               avatar = "/resources/getResource",
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
        private static byte[] GetHash(string password)
        {
            byte[] pwdHash;
            pwdHash = ASCIIEncoding.ASCII.GetBytes(password);

            byte[] pwdSource = new MD5CryptoServiceProvider().ComputeHash(pwdHash);
            return pwdSource;
        }

        public async Task OnPost()
        {

            Console.WriteLine("POSTED!");
            model.password = ByteArrayToString(GetHash(model.password));
            if (_communicator.CreateUser(model).Result.Result == "Added user successfully")
            {
                await _logger.CreateLog("Authorization", HttpContext.User.FindFirst(ClaimTypes.Email).Value + " Вошёл в аккаунт!");
                RedirectToPage("/Employes");
            }
            RedirectToPage("/Employes");
        }
        
        private string ByteArrayToString(byte[] arrInput)
        {
            int i;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);
            for (i = 0; i < arrInput.Length - 1; i++)
            {
                sOutput.Append(arrInput[i].ToString("X2"));
            }
            return sOutput.ToString();
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

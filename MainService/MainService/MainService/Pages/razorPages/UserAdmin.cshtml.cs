using MainService.Communicators;
using MainService.Pages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace MainService.Pages.razorPages
{
    [Authorize]
    [IgnoreAntiforgeryToken]
    public class UserAdminModel : PageModel
    {
        private AuthDbCommunicator _authDbCommunicator = new AuthDbCommunicator();
        [BindProperty(SupportsGet = true)]
        public UserModel currUserModel { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public UserModel userModel { get; set; }
        public IActionResult OnPost(IFormFile currFile)
        {
            try
            {
                Console.WriteLine(Upload);
                Console.WriteLine(currFile);
                Console.WriteLine("adding file!");
                var currDir = "/resources/" + Guid.NewGuid();
                if (!Directory.Exists(currDir))
                {
                    Directory.CreateDirectory(currDir);
                }

                if (Upload.FileName.Contains(".jpg") || Upload.FileName.Contains(".png"))
                {

                    var filePath = Path.Combine(currDir, Upload.FileName);
                    using (var inStream = Upload.OpenReadStream())
                    using (var outStream = new MemoryStream())
                    using (var image = Image.Load(inStream))
                    {
                        image.Mutate(
                            i => i.Resize(165, 165));

                        image.Save(filePath);
                    }

                    Console.WriteLine("adding file with new request");
                    GetReply result = _authDbCommunicator.GetUserByUuId(HttpContext.Session.GetString("SelectedUser")).Result;
                    Guid currentUuid;
                    Guid.TryParse(result.Uuid, out currentUuid);
                    _authDbCommunicator.Modify(new UserModel()
                    {
                        about = result.About,
                        avatar = filePath,
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
                    });
                    return RedirectToPage("/UserAdmin");



                }
                

                return RedirectToPage("/UserAdmin");
            }
            catch
            {
                return RedirectToPage("/UserAdmin");
            }
        }

        public async Task OnGet()
        {

            var result = await _authDbCommunicator.GetUserByUuId(HttpContext.Session.GetString("SelectedUser"));
            Guid currentUuid;
            Guid.TryParse(result.Uuid, out currentUuid);
            userModel = new UserModel()
            {
                about = result.About,
                avatar = "/resources/photoUri?avatar_Url="+result.Avatar,
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
            GetReply result2 = _authDbCommunicator.GetUserByEmail(HttpContext.User.FindFirst(ClaimTypes.Email).Value).Result;

            Guid current1Uuid;
            Guid.TryParse(result.Uuid, out current1Uuid);
            userModel = new UserModel()
            {
                about = result2.About,
                avatar = "/resources/getResource",
                email = result2.Email,
                firstName = result2.FirstName,
                lastName = result2.LastName,
                password = result2.Password,
                patronymic = result2.Patronymic,
                phone = result2.Phone,
                position = result2.Position,
                role = result2.Role,
                telegram = result2.Telegram,
                uuid = current1Uuid,
                dateOfBirth = result2.DateOfBirth
            };
        }

    }
}

using MainService.Communicators;
using MainService.Pages.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Net.Mime;
using System.Security.Claims;
using SixLabors.ImageSharp.Formats;

namespace MainService.Pages.razorPages
{
    [IgnoreAntiforgeryToken]
    [Authorize]
    public class ProfileModel : PageModel
    {
        private AuthDbCommunicator dbCommunicator = new AuthDbCommunicator();
        [BindProperty(SupportsGet = true)] 
        public UserModel userModel { get; set; }
        [BindProperty(SupportsGet = true)]
        public string inviteLetter { get; set; }
        [BindProperty]
        public IFormFile Upload { get; set; }
        [BindProperty]
        public string ErrorMessage {get; set; }
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
                    GetReply result = dbCommunicator.GetUserByEmail(HttpContext.User.FindFirst(ClaimTypes.Email).Value)
                        .Result;
                    Guid currentUuid;
                    Guid.TryParse(result.Uuid, out currentUuid);
                    dbCommunicator.Modify(new UserModel()
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
                    return RedirectToPage("/Profile");



                }
                else
                {
                    ErrorMessage = "Неверный формат файла!";
                }

                return RedirectToPage("/Profile");
            }
            catch
            {
                return RedirectToPage("/Profile");
            }
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
    }
    public class FileUpload
    {
        [Required]
        [Display(Name = "File")]
        public IFormFile FormFile { get; set; }
        public string SuccessMessage { get; set; }
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using MimeKit;

namespace MainService.Controllers
{
    [ApiController]
    [Route("/resources")]
    public class ResourceController : ControllerBase
    {
        public ResourceController()
        {

        }

        /* [Route("photo.png")]
         [HttpGet]
         public async Task<IActionResult> GetPhoto()
         {
             byte[] fileData = System.IO.File.ReadAllBytes("MainService/Pages/img/profile.png");
             string contentType = MimeTypes.GetMimeType(Path.GetExtension("MainService/Pages/img/profile.png"));

             return new FileContentResult(fileData, contentType)
             {
                 FileDownloadName = "photo.png"
             };
         }*/

        [Route("trace.svg")]
        [HttpGet]
        public async Task<IActionResult> GetProfile()
        {
            byte[] fileData = System.IO.File.ReadAllBytes("MainService/Pages/img/trace.svg");
            string contentType = MimeTypes.GetMimeType(Path.GetExtension("MainService/Pages/img/trace.svg"));

            return new FileContentResult(fileData, contentType)
            {
                FileDownloadName = "trace.svg"
            };
        }
        [Route("main_page.css")]
        [HttpGet]
        public async Task GetMainPageCss()
        {
            string page = System.IO.File.ReadAllText("MainService/Pages/css/main_page/main_page.css");
            await HttpContext.Response.WriteAsync(page);
        }

        [Route("styles.css")]
        [HttpGet]
        public async Task GetStyles()
        {
            string page = System.IO.File.ReadAllText("MainService/Pages/css/styles.css");
            await HttpContext.Response.WriteAsync(page);
        }
        [Route("log_in.css")]
        [HttpGet]
        public async Task GetLogIn()
        {
            string page = System.IO.File.ReadAllText("MainService/Pages/css/pop_up/log_in.css");
            await HttpContext.Response.WriteAsync(page);
        }
        [Route("normalize.css")]
        [HttpGet]
        public async Task GetNormalize()
        {
            string page = System.IO.File.ReadAllText("MainService/Pages/css/normalize.css");
            await HttpContext.Response.WriteAsync(page);
        }

        [Route("getresource/{resourceName}")]
        [HttpGet]
        public async Task<IActionResult> GetREs(string resourceName)
        {
            byte[] fileData = System.IO.File.ReadAllBytes($"MainService/Pages/resources/{resourceName}");
            string contentType = MimeTypes.GetMimeType(Path.GetExtension($"MainService/Pages/resources/{resourceName}"));

            return new FileContentResult(fileData, contentType)
            {
                FileDownloadName = resourceName
            };
        }

        [Route("uploadPhoto")]
        [HttpPost]
        public async Task<IActionResult> UploadFile(IFormFile postedFile)
        {
            Console.WriteLine("Fired!!");
            string contentPath = "MainService/Pages/resources/";
            if (!Directory.Exists(contentPath))
            {
                Directory.CreateDirectory(contentPath);
            }
            Console.WriteLine("Addedfile!!");
            string fileName = Path.GetFileName(postedFile.FileName);
            using (FileStream stream = new FileStream(Path.Combine(contentPath, fileName), FileMode.Create))
            {
                postedFile.CopyTo(stream);
            }

            Console.WriteLine("File uploaded");
            return Ok();
        }
    }
}
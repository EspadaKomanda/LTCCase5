using Microsoft.AspNetCore.Mvc;
using System.Net.NetworkInformation;
using System.Security.Claims;
using MainService.Communicators;
using MimeKit;

namespace MainService.Controllers
{
    [ApiController]
    [Route("/resources")]
    public class ResourceController : ControllerBase
    {
        private AuthDbCommunicator _authDbCommunicator = new AuthDbCommunicator();
        public ResourceController()
        {

        }

        [Route("training_modules.css")]
        [HttpGet]
        public async Task GetModules()
        {
            string page = System.IO.File.ReadAllText("MainService/Pages/css/training_modules.css/training_modules.css");
            await HttpContext.Response.WriteAsync(page);
        }
        [Route("header.less")]
        [HttpGet]
        public async Task GetLess()
        {
            string page = System.IO.File.ReadAllText("MainService/Pages/css/main_page/header.less");
            await HttpContext.Response.WriteAsync(page);
        }
        [Route("canvassing.css")]
        [HttpGet]
        public async Task GetCanvassing()
        {
            string page = System.IO.File.ReadAllText("MainService/Pages/css/canvassing/canvassing.css");
            await HttpContext.Response.WriteAsync(page);
        }
        [Route("tick.png")]
        [HttpGet]
        public async Task<IActionResult> GetTick()
        {
            byte[] fileData = System.IO.File.ReadAllBytes("MainService/Pages/img/tick.png");
            string contentType = MimeTypes.GetMimeType(Path.GetExtension("MainService/Pages/img/tick.png"));

            return new FileContentResult(fileData, contentType)
            {
                FileDownloadName = "tick.png"
            };
        }
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

        [Route("getResource")]
        [HttpGet]
        public async Task<IActionResult> GetREs()
        {
            GetReply result = _authDbCommunicator.GetUserByEmail(HttpContext.User.FindFirst(ClaimTypes.Email).Value).Result;
            
            Console.WriteLine("FIRED!!!");
            byte[] fileData = System.IO.File.ReadAllBytes($"{result.Avatar}");
            string contentType = MimeTypes.GetMimeType(Path.GetExtension($"{result.Avatar}"));

            return new FileContentResult(fileData, contentType)
            {
                FileDownloadName = Path.GetFileName($"{result.Avatar}")
            };
        }

      
    }
}
using Microsoft.AspNetCore.Mvc;

namespace MainService.Controllers
{
    [ApiController]
    [Route("/resources")]
    public class ResourceController : ControllerBase
    {
        public ResourceController()
        {

        }

        [Route("styles.css")]
        [HttpGet]
        public async Task GetStyles()
        {
            string page = System.IO.File.ReadAllText("Pages/css/styles.css");
            await HttpContext.Response.WriteAsync(page);
        }
        [Route("log_in.css")]
        [HttpGet]
        public async Task GetLogIn()
        {
            string page = System.IO.File.ReadAllText("Pages/css/pop_up/log_in.css");
            await HttpContext.Response.WriteAsync(page);
        }
        [Route("normalize.css")]
        [HttpGet]
        public async Task GetNormalize()
        {
            string page = System.IO.File.ReadAllText("Pages/css/normalize.css");
            await HttpContext.Response.WriteAsync(page);
        }
    }
}

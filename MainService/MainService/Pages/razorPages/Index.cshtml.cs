
using MainService.Communicators;
using MainService.Pages.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MainService.Pages.razorPages
{
    [IgnoreAntiforgeryToken]
    public class IndexModel : PageModel
    {
        [BindProperty]
        public UserModel userModel { get; set; }

        private ConfigCommunicator _configCommunicator = new ConfigCommunicator();
        private AuthDbCommunicator _authDbCommunicator = new AuthDbCommunicator();
        public async Task<IActionResult> OnPost()
        {
            var Auth = await _configCommunicator.GetAuth();
            var result = await _authDbCommunicator.GetUserByEmail(userModel.email);
            if (result.Password == BCrypt.Net.BCrypt.HashPassword(userModel.password))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, userModel.email) };
                // создаем JWT-токен
                var jwt = new JwtSecurityToken(
                    issuer: Auth.ISSUER,
                audience: Auth.AUDIENCE,
                claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Auth.KEY)), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                
                HttpContext.Session.SetString("Token", encodedJwt);
                return RedirectToPage("/Students/TimeTable");
            }
            Console.WriteLine(userModel.email + ":" + userModel.password);
        }
        public void OnGet()
        {

        }
    }
}

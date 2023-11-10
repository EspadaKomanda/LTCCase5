using MainService.Communicators;
using MainService.Pages.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Generators;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
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
            Console.WriteLine(ByteArrayToString(GetHash(userModel.password)));
            if (result.Password== ByteArrayToString(GetHash(userModel.password)))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Email, userModel.email)};
                
                var jwt = new JwtSecurityToken(
                    issuer: Auth.ISSUER,
                    audience: Auth.AUDIENCE,
                    claims: claims,
                    expires: DateTime.UtcNow.Add(TimeSpan.FromDays(1)),
                    signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Auth.KEY)), SecurityAlgorithms.HmacSha256));
                var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
                Console.WriteLine("Created token!");
                HttpContext.Session.SetString("Token", encodedJwt);
                return RedirectToPage("Profile");
            }
            else
            {
                return BadRequest();
            }
        }
        private static byte[] GetHash(string password)
        {
            byte[] pwdHash;
            pwdHash = ASCIIEncoding.ASCII.GetBytes(password);

            byte[] pwdSource = new MD5CryptoServiceProvider().ComputeHash(pwdHash);
            return pwdSource;
        }

        private static bool CompareHash(byte[] currentpassword, byte[] storedpassword)
        {
            bool bEqual = false;
            if (currentpassword.Length == storedpassword.Length)
            {
                int i = 0;
                while ((i < currentpassword.Length) && (currentpassword[i] == storedpassword[i]))
                {
                    i += 1;
                }
                if (i == currentpassword.Length)
                {
                    bEqual = true;
                }
            }
            return bEqual;
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
        public void OnGet()
        {

        }
    }
}
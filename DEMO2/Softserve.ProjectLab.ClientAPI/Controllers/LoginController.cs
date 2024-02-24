using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Softserve.ProjectLab.ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IConfiguration _config) : Controller
    {
        private static List<string[]> userPass =
        [
            ["User", "pass"],
            ["admin", "admin"],
        ];

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(string user, string pass)
        {
            bool found = userPass.Any(x => x[0].Equals(user) && x[1].Equals(pass));
            if (!found)
            {
                return Unauthorized("Incorrect credentials");
            }
            
            return Ok(GenerateToken(user));
        }
        private string GenerateToken(string user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            JwtSecurityToken token = new(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                [new Claim(ClaimTypes.NameIdentifier, user)],
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

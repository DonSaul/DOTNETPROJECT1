using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Softserve.ProjectLab.ClientAPI.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Softserve.ProjectLab.ClientAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(IConfiguration _config) : Controller
    {
        private static List<LoginBody> userPass =
        [
            new LoginBody("User", "pass"),
            new LoginBody("admin", "admin"),
        ];

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] LoginBody body)
        {
            bool found = userPass.Any(x => x.User.Equals(body.User) && x.Pass.Equals(body.Pass));
            if (!found)
            {
                return Unauthorized("Incorrect credentials");
            }
            return Json(new { token = GenerateToken(body.User) });
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

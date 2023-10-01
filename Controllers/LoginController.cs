using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PROJECT_MANAGER_WEB_API.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace PROJECT_MANAGER_WEB_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _configuration;
        public LoginController(IConfiguration configuration)
        {

           this._configuration = configuration;

        }

        // Authenticate the user
        [HttpPost]
        private Users AuthenticateUser(Users user)
        {
            Users _user = null;
            if (user != null)
            {
                if (user.Username == "admin" && user.Password == "pass@123") 
                {
                    _user = new Users { Username = "Akash kumar",
                        role = "admin",
                        email = "araushan11@gmail.com",
                    };
                }
            }

            return _user;
        }

        // It will generate token for user
        private string GenerateToken(Users user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._configuration["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(this._configuration["Jwt:Issuer"], this._configuration["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(30), signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Loging method to authenticate user and generate token for user
        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login(Users user)
        {
            IActionResult response = Unauthorized();
            var user_ = AuthenticateUser(user);
            
            if(user_ != null)
            {
                var token = GenerateToken(user_);
                response = Ok(new { token = token });
            }

            return response;
        }
    }
}

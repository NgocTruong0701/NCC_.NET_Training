using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System.Buffers.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace API_1.Controllers
{
    [Route("login")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        public IConfiguration _configuration;
        public UserInfor _currentUser;

        public TokenController(IConfiguration config)
        {
            _configuration = config;
            // mock an user
            _currentUser = new UserInfor()
            {
                Email = "foo@gmail.com",
                FirstName = "foo",
                LastName = "bar",
                Password = "password",
                UserId = 1,
                UserName = "foo bar",
                CreatedDate = DateTime.Now,
            };
        }

        [HttpPost]
        public IActionResult POST(UserInfor _userData)
        {
            if (_userData != null && _userData.Email != null && _userData.Password != null)
            {
                var user = GetUser(_userData.Email, _userData.Password);

                if (user != null)
                {
                    // create claims details based on the user information
                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.Sub, _configuration["Jwt:Subject"]),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                        new Claim("Id", user.UserName.ToString()),
                        new Claim("FirstName", user.FirstName),
                        new Claim("LastName", user.LastName),
                        new Claim("UserName", user.UserName),
                        new Claim("Email", user.Email),
                    };

                    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

                    var signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                    var token = new JwtSecurityToken(_configuration["Jwt:Jssuer"], _configuration["Jwt:Audience"], claims, expires: DateTime.UtcNow.AddDays(1), signingCredentials: signIn);

                    return Ok(new JwtSecurityTokenHandler().WriteToken(token));
                }
                else
                {
                    return BadRequest("Invalid credentials");
                }
            }
            else
            {
                return BadRequest();
            }
        }

        private UserInfor GetUser(string email, string password)
        {
            if (_currentUser.Email != email || _currentUser.Password != password)
                return null;
            return _currentUser;
        }
    }
}

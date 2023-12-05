using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Realtime.Entity;
using Realtime.IService;
using Realtime.Service;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Realtime.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly ILogin _ilogin;
        public readonly IConfiguration _configuration;
        private readonly AppdbContext dbcontext;
        public LoginController(IConfiguration configuration)
        {
            _ilogin = new LoginService();
            _configuration = configuration;
            dbcontext = new AppdbContext();

        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var res = await _ilogin.LoginAsync(userName, password);
            if (res == Enum.ErrorHelper.userNameNull)
            {
                return BadRequest("Tên Tài Khoản trống");
            }
            else if (res == Enum.ErrorHelper.passWordNull)
            {
                return BadRequest("Mật khẩu  trống");

            }
            else if (res == Enum.ErrorHelper.loginThanhCong)
            {
                var user = dbcontext.users.FirstOrDefault(x => x.Email == userName);
                string token = GenerateTocken(user);
                return Ok(token);
            }
            else
            {
                return BadRequest("Tài khoản không tồn tại");

            }
        }

        private string GenerateTocken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("userId", user.Id.ToString()),
                new Claim("Name",user.TenUser.ToString()),
            };
            var sercuritykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSettings:Key"]));
            var credentials = new SigningCredentials(sercuritykey, SecurityAlgorithms.HmacSha256);

            var tocken = new JwtSecurityToken(_configuration["JwtSettings:Issuer"], _configuration["JwtSettings:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(tocken);
        }
    }
}

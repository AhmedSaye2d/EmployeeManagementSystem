using BaseLibrary.DTOs;
using HostLibrary.Repository.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserAccount userAccount) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(Register user)
        {
            if(user == null)
            {
                return BadRequest("User data is null.");
            }
            var res=await userAccount.CreateAsync(user);
            return Ok(res);
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(Login user)
        {
            if (user == null)
            {
                return BadRequest("Login data is null.");
            }
            var res = await userAccount.LoginAsync(user);
            return Ok(res);
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(RefreshToken token)
        {
            if (token == null)
            {
                return BadRequest("Refresh token is null.");
            }
            var res = await userAccount.RefreshTokenAsync(token);
            return Ok(res);
        }
    }
}

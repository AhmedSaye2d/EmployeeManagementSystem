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
    }
}

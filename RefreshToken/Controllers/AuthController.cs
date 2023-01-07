using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefreshToken.Dto;
using RefreshToken.Model;

namespace RefreshToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        //registering for user
        [HttpPost]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var user = new User { UserName = request.UserName };
            return Ok(user);
        }
    }
}

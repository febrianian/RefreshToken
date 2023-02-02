using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RefreshToken.Dto;
using RefreshToken.Model;
using RefreshToken.Services.AuthServices;

namespace RefreshToken.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthServices _authServices;

        public AuthController(IAuthServices authServices)
        {
            _authServices = authServices;
        }

        //registering for user
        [HttpPost]
        public async Task<ActionResult<User>> Register(UserDto request)
        {
            var response = await _authServices.RegisterUser(request);
            return Ok(response);
        }

        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login(UserDto request)
        {
            var response = await _authServices.Login(request);

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpPost("refresh-token")]
        public async Task<ActionResult<string>> AuthRefreshToken()
        {
            var response = await _authServices.AuthRefreshToken();

            if (response.Success)
            {
                return Ok(response);
            }
            else
            {
                return BadRequest(response.Message);
            }
        }

        [HttpGet("HalloAdmin"), Authorize (Roles = "Administrator")]
        public ActionResult<string> HalloAdmin()
        {
            return Ok("Hallo! You're authorized!");
        }

        [HttpGet("HalloCustomer"), Authorize(Roles = "Customer")]
        public ActionResult<string> HalloCustomer()
        {
            return Ok("Hallo! You're authorized!");
        }
    }
}

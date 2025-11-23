using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Abstraction.Contracts;
using Shared.Dtos.Identity;
using Shared.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Presentation.Controllers
{
    public class AuthenticationController(IServiceManger serviceManger) : ApiControllerBase
    {
        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
       => Ok(await serviceManger.AuthenticationService.LoginAsync(loginDto));
        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
            => Ok( await serviceManger.AuthenticationService.RegisterAsync(registerDto));

        [HttpGet("EmailExists")]
        public async Task<ActionResult<bool>> CheckEmailExists(string email)
            => Ok( await serviceManger.AuthenticationService.CheckEmaiExists(email));

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await serviceManger.AuthenticationService.GetUserByEmail(email));
        }

        [Authorize]
        [HttpGet("Address")]
        public async Task<ActionResult<AddressDto>> GetAddress()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await serviceManger.AuthenticationService.GetUserAddress(email));
        }

        [Authorize]
        [HttpPut("Address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
        {
            var email = User.FindFirstValue(ClaimTypes.Email);
            return Ok(await serviceManger.AuthenticationService
                .UpdateUserAddress(email,addressDto));
        }

    }
}

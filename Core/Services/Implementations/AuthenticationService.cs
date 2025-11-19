using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Services.Abstraction.Contracts;
using Shared.Dtos.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    internal class AuthenticationService(UserManager<User> userManager) : IAuthenticationService
    {
        public async Task<UserDto> LoginAsync(LoginDto loginDto)
        {
            var user = await userManager.FindByEmailAsync(loginDto.Email);
            if (user == null)
            throw new UnAuthorizedException($"Email {loginDto.Email} Is Not Exist");
            var result = await userManager.CheckPasswordAsync(user, loginDto.Password);
            if (!result)
                throw new UnAuthorizedException();
            return new UserDto(
                user.DisplayName,
                user.Email!,
                "token"
                );  
        }

        public async Task<UserDto> RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                DisplayName = registerDto.DisplayName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PhoneNumber = registerDto.PhoneNumber
            };
            var result =await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
               var errors = result.Errors.Select(error=> error.Description).ToList();   
                throw new ValidationsExceptions(errors);
            }
            return new UserDto(
                user.DisplayName,
                user.Email!,
                "token"
                );
        }
    }
}

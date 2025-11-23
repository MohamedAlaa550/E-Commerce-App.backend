using AutoMapper;
using Domain.Entities.IdentityModule;
using Domain.Exceptions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Services.Abstraction.Contracts;
using Shared;
using Shared.Dtos.Identity;
using Shared.Dtos.Order;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    internal class AuthenticationService(UserManager<User> userManager
        ,IOptions<JwtOptions> options, IMapper mapper)
        : IAuthenticationService
    {
        public async Task<bool> CheckEmaiExists(string email)
        {
            var user = await userManager.FindByEmailAsync(email);
            return user != null;
        }

        public async Task<AddressDto> GetUserAddress(string email)
        {
            var user =await userManager.Users.Include(u => u.Adress)
                .FirstOrDefaultAsync(u => u.Email == email) ??
                throw new UserNotFoundExcpetion(email);
            return new AddressDto()
            {
                FirstName = user.Adress?.FirstOrDefault()?.FirstName!,
                LastName = user.Adress?.FirstOrDefault()?.LastName!,
                Street = user.Adress?.FirstOrDefault()?.Street!,
                City = user.Adress?.FirstOrDefault()?.City!,
                Country = user.Adress?.FirstOrDefault()?.Country!
            };

        }

        public async Task<UserDto> GetUserByEmail(string email)
        {
            var user =await userManager.FindByEmailAsync(email) ??
                    throw new UserNotFoundExcpetion(email);
            return new UserDto(
                user.DisplayName,
                user.Email!,
               await CreateTokenAsync(user)
                );
        }

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
                await CreateTokenAsync(user)
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
            var result = await userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(error => error.Description).ToList();
                throw new ValidationsExceptions(errors);
            }
            return new UserDto(
                user.DisplayName,
                user.Email!,
             await CreateTokenAsync(user)
                );
        }

        public async Task<AddressDto> UpdateUserAddress(string email, AddressDto addressDto)
        {
            var user = await userManager.Users.Include(u => u.Adress)
                .FirstOrDefaultAsync(u => u.Email == email) ??
                throw new UserNotFoundExcpetion(email);
            var adress = user.Adress?.FirstOrDefault();
            if (adress != null)
            {
                adress.FirstName = addressDto.FirstName;
                adress.LastName = addressDto.LastName;
                adress.Street = addressDto.Street;
                adress.City = addressDto.City;
                adress.Country = addressDto.Country;
            }

            else
            {
                var userAddress = mapper.Map<UserAddress>(addressDto);
                user.Adress = new List<Adress> { userAddress };
            }
            await userManager.UpdateAsync(user);
            return new AddressDto()
            {
                FirstName = user.Adress?.FirstOrDefault()?.FirstName!,
                LastName = user.Adress?.FirstOrDefault()?.LastName!,
                Street = user.Adress?.FirstOrDefault()?.Street!,
                City = user.Adress?.FirstOrDefault()?.City!,
                Country = user.Adress?.FirstOrDefault()?.Country!
            };
        }

        private async Task<string> CreateTokenAsync(User user)
        {
            var jwtOptions = options.Value;
            var authClaims = new List<Claim>()
            {
               new Claim(ClaimTypes.Name,user.DisplayName),
               new Claim(ClaimTypes.Email,user.Email!)
            };

            var roles = await userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }
            var key = new SymmetricSecurityKey
                (Encoding.UTF8.GetBytes(jwtOptions.SecretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwtOptions.Issuer,
                audience: jwtOptions.Audience,
                expires: DateTime.Now.AddDays(jwtOptions.DurationInDays),
                claims: authClaims,
                signingCredentials: creds
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

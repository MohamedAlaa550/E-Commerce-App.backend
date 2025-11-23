using Shared.Dtos.Identity;
using Shared.Dtos.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Abstraction.Contracts
{
    public interface IAuthenticationService
    {
        public Task<UserDto> LoginAsync(LoginDto loginDto);
        public Task<UserDto> RegisterAsync(RegisterDto registerDto);

        public Task<UserDto> GetUserByEmail (string email);

        public Task<bool> CheckEmaiExists (string email);

        public Task<AddressDto> GetUserAddress (string email);

        public Task<AddressDto> UpdateUserAddress (string email, AddressDto addressDto);
    }
}

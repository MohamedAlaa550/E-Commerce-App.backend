using Shared.Dtos.Identity;
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
    }
}

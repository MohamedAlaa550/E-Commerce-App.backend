using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Identity
{
    public record UserDto(string DisplayName, string Email, string Token)
    {

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Dtos.Identity
{
    public record RegisterDto
    {
        public string DisplayName { get; init; }
        public string UserName { get; init; }
        [EmailAddress]
        public string Email { get; init; }
        public string Password { get; init; }
        public string PhoneNumber { get; init; }

    }
}

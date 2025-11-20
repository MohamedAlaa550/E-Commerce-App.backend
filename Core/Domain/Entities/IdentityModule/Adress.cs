using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities.IdentityModule
{
    public class Adress
    {
        public int Id { get; set; }
        public string Street { get; set; } = null!;
        public string City { get; set; } = null!;
        public string Country { get; set; } = null!;
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public User User { get; set; }

        public string UserId { get; set; } = null!;



    }
}

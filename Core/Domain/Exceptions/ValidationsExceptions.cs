using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Exceptions
{
    public class ValidationsExceptions : Exception
    {
        public IEnumerable<string> Errors { get; set; }
        public ValidationsExceptions(IEnumerable<string> errors)
            : base("Validation Failed")
        {
            Errors = errors;
        }
    }
}

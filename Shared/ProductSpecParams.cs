using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductSpecParams
    {
        public ProductSortringOptions? sort { get; set; }
        public int? TppeId { get; set; }
        public int? BrandId { get; set; }
    }
}

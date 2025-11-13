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
        public int? TypeId { get; set; }
        public int? BrandId { get; set; }

        private const int MaxPageSize = 10;

        private const int DefaultPageSize = 5;

        public int pageIndex { get; set; } = 1;

        private int PageSize = DefaultPageSize;
        public int pageSize
        {
            get { return PageSize; }
            set { PageSize = (value > MaxPageSize) ? MaxPageSize : value; }
        }


    }
}

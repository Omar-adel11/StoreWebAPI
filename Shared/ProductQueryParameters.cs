using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class ProductQueryParameters
    {
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? sort { get; set; }
        public string? search { get; set; }
        public int pageIndex { get; set; } = 1;
        public int pageSize { get; set; } = 5;
    }
}

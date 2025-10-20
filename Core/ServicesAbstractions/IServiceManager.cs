using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServicesAbstractions.Products;

namespace ServicesAbstractions
{
    public class IServiceManager
    {
        IProductService productService { get; }
    }
}

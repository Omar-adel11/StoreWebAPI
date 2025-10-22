using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Shared;

namespace Services.Specifications
{
    public class ProductsCountSpecifications : BaseSpecifications <int,Product>
    {
        public ProductsCountSpecifications(ProductQueryParameters parameters) : base
           (
               p => (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId)
               && (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId)
               && (string.IsNullOrEmpty(parameters.search) || p.Name.ToLower().Contains(parameters.search.ToLower()))

           )
        {

        }
    }
}

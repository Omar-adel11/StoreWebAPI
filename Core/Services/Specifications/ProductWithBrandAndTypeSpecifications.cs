using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;
using Shared;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<int, Product>
    {
        public ProductWithBrandAndTypeSpecifications(ProductQueryParameters parameters) : base
            (
                p=>(!parameters.BrandId.HasValue ||p.BrandId == parameters.BrandId)
                && (! parameters.TypeId.HasValue ||p.TypeId == parameters.TypeId)
                && (string.IsNullOrEmpty(parameters.search) || p.Name.ToLower().Contains(parameters.search.ToLower()))

            )
        {
            ApplyPagination(parameters.pageSize, parameters.pageIndex);
            ApplySorting(parameters.sort);
            ApplyIncludes();
        }

        public ProductWithBrandAndTypeSpecifications(int id) : base(p=>p.Id == id)
        {
            ApplyIncludes();
        }

        private void ApplySorting(string? sort)
        {
            if (!string.IsNullOrEmpty(sort))
            {
                switch (sort.ToLower())
                {
                    case "priceasc":
                        OrderBy = p => p.Price;
                        break;
                    case "pricedesc":
                        OrderBydescending = p => p.Price;
                        break;
                    default:
                        OrderBy = p => p.Name;
                        break;
                }
            }
            else
            {
                OrderBy = p => p.Name;
            }
        }
        private void ApplyIncludes()
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Entities.Product;

namespace Services.Specifications
{
    public class ProductWithBrandAndTypeSpecifications : BaseSpecifications<int, Product>
    {
        public ProductWithBrandAndTypeSpecifications(Expression<Func<Product,bool>>? expression) : base(expression)
        {
            Includes.Add(p => p.Brand);
            Includes.Add(p => p.Type);
        }
        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shared;
using Shared.DTOs.Products;

namespace ServicesAbstractions.Products
{
    public interface IProductService
    {
        Task<PaginationResponse<ProductResponse>> GetAllProductAsync(ProductQueryParameters parameters);

        Task<ProductResponse> GetProductByIdAsync(int id);
        Task<IEnumerable<BrandAndTypeResponse>> GetAllBrandsAsync();
        Task<IEnumerable<BrandAndTypeResponse>> GetAllTypesAsync();
    }
}

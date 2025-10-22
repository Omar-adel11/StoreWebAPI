using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Product;
using Services.Specifications;
using ServicesAbstractions.Products;
using Shared;
using Shared.DTOs.Products;

namespace Services.Products
{
    public class ProductService(IUnitOfWork _unitOfWork, IMapper _mapper) : IProductService
    {

        public async Task<PaginationResponse<ProductResponse>> GetAllProductAsync(ProductQueryParameters parameters)
        {
            //var spec = new BaseSpecifications<int, Product>(null);
            //spec.Includes.Add(p => p.Brand);
            //spec.Includes.Add(p => p.Type);

            var spec = new ProductWithBrandAndTypeSpecifications(parameters);
            var products = await _unitOfWork.GetRepository<int, Product>().GetAllAsync(spec);
            var specCount = new ProductsCountSpecifications(parameters);

            var count = await _unitOfWork.GetRepository<int, Product>().GetCountAsync(specCount);

            var result = _mapper.Map<IEnumerable<ProductResponse>>(products);
            return new PaginationResponse<ProductResponse>(parameters.pageSize, parameters.pageIndex, count, result);
        }
        public async Task<ProductResponse> GetProductByIdAsync(int id)
        {
            var spec = new ProductWithBrandAndTypeSpecifications(id);
            var product = await _unitOfWork.GetRepository<int, Product>().GetByIdAsync(spec);
            //var product = await _unitOfWork.GetRepository<int, Product>().GetByIdAsync(id);
            var result = _mapper.Map<ProductResponse>(product); 
            return result;
        }
        public async Task<IEnumerable<BrandAndTypeResponse>> GetAllBrandsAsync()
        {
            var Brands = await  _unitOfWork.GetRepository<int, ProductBrand>().GetAllAsync();
            var result = _mapper.Map<IEnumerable<BrandAndTypeResponse>>(Brands);
            return result;
        }

        public async Task<IEnumerable<BrandAndTypeResponse>> GetAllTypesAsync()
        {
            var Types = await _unitOfWork.GetRepository<int, ProductType>().GetAllAsync();
            var result = _mapper.Map< IEnumerable<BrandAndTypeResponse >> (Types);
            return result;
        }

      
    }
}

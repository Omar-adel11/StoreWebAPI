using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Entities.Product;
using Microsoft.Extensions.Configuration;
using Shared.DTOs.Products;

namespace Services.Mapping.Products
{
    public class ProductsProfile : Profile
    {
        public ProductsProfile(IConfiguration configuration)
        {
            CreateMap<Product, ProductResponse>()
                .ForMember(dest => dest.Brand, o => o.MapFrom(src => src.Brand.Name))
                .ForMember(dest => dest.Type, o => o.MapFrom(src => src.Type.Name))
                .ForMember(dest => dest.PictureUrl, o => o.MapFrom(new ProductPictureUrlResolver(configuration)))
                ;

            CreateMap<ProductBrand, BrandAndTypeResponse>();
            CreateMap<ProductType, BrandAndTypeResponse>();
        }
    }
}

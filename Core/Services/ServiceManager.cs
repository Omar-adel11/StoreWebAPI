using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Services.Basket;
using Services.Products;
using ServicesAbstractions;
using ServicesAbstractions.Baskets;
using ServicesAbstractions.Products;



namespace Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _BasketRepository) : IServiceManager
    {
        public IProductService productService { get; } = new ProductService(_unitOfWork, _mapper);

        public IBasketService basketService { get; } = new BasketService(_BasketRepository, _mapper);
    }
}

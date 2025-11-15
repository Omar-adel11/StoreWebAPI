using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Contracts;
using Domain.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Basket;
using Services.Identity;
using Services.Order;
using Services.Products;
using ServicesAbstractions;
using ServicesAbstractions.Baskets;
using ServicesAbstractions.Identity;
using ServicesAbstractions.Orders;
using ServicesAbstractions.Products;
using Shared;



namespace Services
{
    public class ServiceManager(IUnitOfWork _unitOfWork,
        IMapper _mapper,
        IBasketRepository _BasketRepository,
        ICacheRepository cacheRepository,
        IOptions<JWTOptions> _options,
        UserManager<Appuser> _userManager) : IServiceManager
    {
        public IProductService productService { get; } = new ProductService(_unitOfWork, _mapper);

        public IBasketService basketService { get; } = new BasketService(_BasketRepository, _mapper);

        public ICacheService cacheService { get; } = new CacheService(cacheRepository);


        public IAuthService authService { get; } = new AuthService(_userManager, _options,_mapper);

        public IOrderService orderService{ get; } = new OrderService(_unitOfWork, _mapper,_BasketRepository);

    }
}

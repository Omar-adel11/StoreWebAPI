using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Mapping.Basket;
using Services.Mapping.Order;
using Services.Mapping.Products;
using Services.Products;
using ServicesAbstractions;
using ServicesAbstractions.Products;
using Shared;

namespace Services
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection Services,IConfiguration Configuration)
        {
            Services.AddScoped<IProductService, ProductService>();
            

            //Services.AddAutoMapper(m => m.AddProfile(new ProductsProfile(builder.Configuration)));
            Services.AddAutoMapper(m => m.AddProfile(new ProductsProfile(Configuration)));
            Services.AddAutoMapper(m => m.AddProfile(new BasketProfile()));
            Services.AddAutoMapper(m => m.AddProfile(new OrderProfile()));

            Services.Configure<JWTOptions>(Configuration.GetSection("JWTOptions"));

            return Services;
        }
    }
}

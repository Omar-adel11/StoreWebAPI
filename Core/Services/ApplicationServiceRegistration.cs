using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Services.Mapping.Basket;
using Services.Mapping.Products;
using Services.Products;
using ServicesAbstractions;
using ServicesAbstractions.Products;

namespace Services
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationService(this IServiceCollection Services,IConfiguration Configuration)
        {
            Services.AddScoped<IProductService, ProductService>();
            Services.AddScoped<IServiceManager, ServiceManager>();
            
            //Services.AddAutoMapper(m => m.AddProfile(new ProductsProfile(builder.Configuration)));
            Services.AddAutoMapper(m => m.AddProfile(new ProductsProfile(Configuration)));
            Services.AddAutoMapper(m => m.AddProfile(new BasketProfile()));

            return Services;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Persistance.Data.Contexts;
using Persistance.Repositories;
using Services;
using ServicesAbstractions;
using StackExchange.Redis;

namespace Persistance
{
    public static class InfrastructureServicesRegistration
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection Services, IConfiguration Configuration)
        {
            Services.AddDbContext<StoreDbContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            });

            Services.AddScoped<IDbInitializer, DbInitializer>();
            Services.AddScoped<IUnitOfWork, UnitOfWork>();
            Services.AddScoped<IBasketRepository, BasketRepository>();
            Services.AddScoped<IServiceManager, ServiceManager>();
            Services.AddScoped<ICacheRepository, CacheRepository>();
            Services.AddSingleton<IConnectionMultiplexer>((ServiceProvider) =>
            {
                return ConnectionMultiplexer.Connect(Configuration.GetConnectionString("Redis")!);
            });

            return Services;
        }
    }
}

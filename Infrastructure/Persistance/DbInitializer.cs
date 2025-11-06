using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.Identity;
using Domain.Entities.Order;
using Domain.Entities.Product;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Contexts;
using Persistance.Identity;

namespace Persistance
{
    public class DbInitializer(StoreDbContext _context,
        StoreIdentityDbContext _storeIdentityDbContext,
        UserManager<Appuser> _userManager,
        RoleManager<IdentityRole> _roleManager
        ) : IDbInitializer
    {

        public async Task InitializeAsync()
        {
            //Create Database
            //Update Database
            if (_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _context.Database.MigrateAsync();

            }
            //Seed Data
            //Infrastructure\Persistance\Data\DataSeeding

            if (!_context.ProductBrands.Any())
            {
                //1. ProductBrands
                //1.read all data
                var BrandData = await File.ReadAllTextAsync(@"../Infrastructure/Persistance/Data/DataSeeding/brands.json");
                //2.Deserialize
                var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);
                //3.check if data exist then add list to database
                if (Brands is not null && Brands.Count > 0)
                {
                    await _context.ProductBrands.AddRangeAsync(Brands);

                }
            }


            //2. ProductTypes
            if (!_context.ProductTypes.Any())
            {
                //1. ProductBrands
                //1.read all data
                var TypeData = await File.ReadAllTextAsync(@"../Infrastructure/Persistance/Data/DataSeeding/types.json");
                //2.Deserialize
                var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);
                //3.check if data exist then add list to database
                if (Types is not null && Types.Count > 0)
                {
                    await _context.ProductTypes.AddRangeAsync(Types);

                }
            }
            //3. Products
            if (!_context.Products.Any())
            {
                //1. ProductBrands
                //1.read all data
                var ProductsData = await File.ReadAllTextAsync(@"../Infrastructure/Persistance/Data/DataSeeding/products.json");
                //2.Deserialize
                var products = JsonSerializer.Deserialize<List<Product>>(ProductsData);
                //3.check if data exist then add list to database
                if (products is not null && products.Count > 0)
                {
                    await _context.Products.AddRangeAsync(products);

                }

            }

            if (!_context.DeliveryMethods.Any())
            {
                //1. DeliveryMethods
                //1.read all data
                var DeliveryData = await File.ReadAllTextAsync(@"../Infrastructure/Persistance/Data/DataSeeding/delivery.json");
                //2.Deserialize
                var DeliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(DeliveryData);
                //3.check if data exist then add list to database
                if (DeliveryMethods is not null && DeliveryMethods.Count > 0)
                {
                    await _context.DeliveryMethods.AddRangeAsync(DeliveryMethods);

                }
            }


            await _context.SaveChangesAsync();

        }

        public async Task InitializeIdentityAsync()
        {
            if (_storeIdentityDbContext.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _storeIdentityDbContext.Database.MigrateAsync();
            }

            //Seed Roles
            if (_roleManager.Roles.Any() is false)
            {
                await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }
            //Seed Users
            if (!_userManager.Users.Any())
            {
                var AdminUser = new Appuser
                {
                    DisplayName = "Admin",
                    UserName = "adminuser",
                    Email = "omaradel1258@gmail.com",
                    PhoneNumber = "01287276101"
                };
                var SuperAdminUser = new Appuser
                {
                    DisplayName = "SuperAdmin",
                    UserName = "superadminuser",
                    Email = "omaradel1258@gmail.com",
                    PhoneNumber = "01287276101"
                };

                await _userManager.CreateAsync(AdminUser, "P@ssw0rd");
                await _userManager.CreateAsync(SuperAdminUser, "P@ssw0rd");
                

                await _userManager.AddToRoleAsync(AdminUser, "Admin");
                await _userManager.AddToRoleAsync(SuperAdminUser, "SuperAdmin");

            }
        }
    }
}

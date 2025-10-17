using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Entities.Product;
using Microsoft.EntityFrameworkCore;
using Persistance.Data.Contexts;

namespace Persistance
{
    public class DbInitializer(StoreDbContext _context) : IDbInitializer
    {

        public async Task InitializeAsync()
        {
            //Create Database
            //Update Database
            if(_context.Database.GetPendingMigrationsAsync().GetAwaiter().GetResult().Any())
            {
                await _context.Database.MigrateAsync();

            }
            //Seed Data
            //Infrastructure\Persistance\Data\DataSeeding

            if(! _context.ProductBrands.Any())
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

            await _context.SaveChangesAsync();

        }
    }
}

using Domain.Contracts;
using Domain.Entities.ProductModule;
using Microsoft.EntityFrameworkCore;
using Persistence.Data.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Persistence.Data
{
    public class DbInitializer : IDbInitializer
    {
        private readonly StoreDbContext _dbContext;

        public DbInitializer(StoreDbContext dbContext) 
        {
            _dbContext = dbContext;
        }
        public async Task InitializeAsync()
        {
            try
            {
                if (_dbContext.Database.GetPendingMigrations().Any())
                    await _dbContext.Database.MigrateAsync();

                if (!_dbContext.ProductTypes.Any())
                {
                    var TypeData = await File.ReadAllTextAsync(@"..\InfraStructure\Persistence\Data\DataSeeding\types.json");
                    var Types = JsonSerializer.Deserialize<List<ProductType>>(TypeData);

                    if (Types is not null && Types.Any())
                    {
                        await _dbContext.ProductTypes.AddRangeAsync(Types);

                    }
                }

                if (!_dbContext.ProductBrands.Any())
                {
                    var BrandData = await File.ReadAllTextAsync(@"..\InfraStructure\Persistence\Data\DataSeeding\brands.json");
                    var Brands = JsonSerializer.Deserialize<List<ProductBrand>>(BrandData);

                    if (Brands is not null && Brands.Any())
                    {
                        await _dbContext.ProductBrands.AddRangeAsync(Brands);

                    }
                }

                if (!_dbContext.Products.Any())
                {
                    var ProductData = await File.ReadAllTextAsync(@"..\InfraStructure\Persistence\Data\DataSeeding\products.json");
                    var Products = JsonSerializer.Deserialize<List<Product>>(ProductData);
                    if (Products is not null && Products.Any())
                    {
                        await _dbContext.Products.AddRangeAsync(Products);

                    }
                }
                await _dbContext.SaveChangesAsync();
            }
            catch (global::System.Exception)
            {

                throw;
            }

        }
	
    }
}

using Domain.Contracts;
using Domain.Entities.IdentityModule;
using Domain.Entities.ProductModule;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(StoreDbContext dbContext, UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager) 
        {
            _dbContext = dbContext;
            _roleManager = roleManager;
            _userManager = userManager;
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

        public async Task InitializeIdentityAsync()
        {
            if (!_roleManager.Roles.Any())
            {
               await _roleManager.CreateAsync(new IdentityRole("Admin"));
                await _roleManager.CreateAsync(new IdentityRole("SuperAdmin"));
            }

            if (!_userManager.Users.Any())
            {
                var adminUser = new User()
                {
                    DisplayName = "Admin",
                    Email = "Admin@gmail.com",
                    UserName = "Admin",
                    PhoneNumber = "01016747735",
                };

                var superAdmin = new User()
                {
                    DisplayName = "SuperAdmin",
                    Email = "SuperAdmin@gmail.com",
                    UserName = "SuperAdmin",
                    PhoneNumber = "01101811301",
                };
                await _userManager.CreateAsync(adminUser, "Admin@123");
                await _userManager.CreateAsync(superAdmin, "SuperAdmin@123");
                await _userManager.AddToRoleAsync(adminUser, "Admin");
                await _userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
            }
        }
    }
}

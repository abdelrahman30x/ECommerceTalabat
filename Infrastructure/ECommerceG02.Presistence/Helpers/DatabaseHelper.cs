using ECommerceG02.Domian.Models.Identity;
using ECommerceG02.Domian.Models.Orders;
using ECommerceG02.Domian.Models.Products;
using ECommerceG02.Presistence.Contexts;
using ECommerceG02.Presistence.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ECommerceG02.Presistence.Helpers
{
    public class DatabaseHelper : IDatabaseHelper
    {
        private readonly StoreDbContext _context;
        private readonly StoreIdentityDbContext _identityContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DatabaseHelper(
            StoreDbContext context,
            StoreIdentityDbContext identityContext,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _identityContext = identityContext;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task InitializeAsync()
        {
            await MigrateDatabasesAsync();
            await SeedProductsAndDeliveryAsync();
            await SeedRolesAsync();
            await SeedSuperAdminAsync();
            await SeedDemoUsersAsync();
        }

        #region Migrations
        private async Task MigrateDatabasesAsync()
        {
            if ((await _context.Database.GetPendingMigrationsAsync()).Any())
                await _context.Database.MigrateAsync();

            if ((await _identityContext.Database.GetPendingMigrationsAsync()).Any())
                await _identityContext.Database.MigrateAsync();
        }
        #endregion

        #region Products & Delivery Seed
        private async Task SeedProductsAndDeliveryAsync()
        {
            // Brands
            if (!_context.ProductBrands.Any())
            {
                var brandsJson = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerceG02.Presistence\Data\brands.json");
                var brands = JsonSerializer.Deserialize<List<ProductBrand>>(brandsJson);
                if (brands?.Any() == true) _context.ProductBrands.AddRange(brands);
            }

            // Types
            if (!_context.ProductTypes.Any())
            {
                var typesJson = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerceG02.Presistence\Data\types.json");
                var types = JsonSerializer.Deserialize<List<ProductType>>(typesJson);
                if (types?.Any() == true) _context.ProductTypes.AddRange(types);
            }

            // Products
            if (!_context.Products.Any())
            {
                var productsJson = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerceG02.Presistence\Data\products.json");
                var products = JsonSerializer.Deserialize<List<Product>>(productsJson);
                if (products?.Any() == true) _context.Products.AddRange(products);
            }

            // Delivery Methods
            if (!_context.DeliveryMethods.Any())
            {
                var deliveryJson = await File.ReadAllTextAsync(@"..\Infrastructure\ECommerceG02.Presistence\Data\deliveryMethods.json");
                var deliveryMethods = JsonSerializer.Deserialize<List<DeliveryMethod>>(deliveryJson);
                if (deliveryMethods?.Any() == true) _context.DeliveryMethods.AddRange(deliveryMethods);
            }

            await _context.SaveChangesAsync();
        }
        #endregion

        #region Identity Seed
        private async Task SeedRolesAsync()
        {
            string[] roles = { "Administrator", "Manager", "Customer", "Vendor", "Support" };
            foreach (var roleName in roles)
            {
                if (!await _roleManager.RoleExistsAsync(roleName))
                    await _roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        private async Task SeedSuperAdminAsync()
        {
            const string email = "superadmin@example.com";
            if (await _userManager.FindByEmailAsync(email) == null)
            {
                var superAdmin = new ApplicationUser
                {
                    UserName = "superadmin",
                    Email = email,
                    EmailConfirmed = true,
                    FirstName = "Super",
                    LastName = "Admin",
                    DateOfBirth = new DateTime(1990, 1, 1),
                    CreatedAt = DateTime.UtcNow.AddYears(-2),
                    LastLoginAt = DateTime.UtcNow.AddDays(-1),
                    IsActive = true,
                    MobileNumber = "+201234567890",
                    AlternativeEmail = "superadmin.alt@example.com",
                    ProfilePictureUrl = "https://example.com/images/superadmin.png",
                    PreferredLanguage = "en",
                    TimeZone = "UTC",
                    Bio = "I am the Super Admin of ECommerceG02 platform.",
                    Address = new Address
                    {
                        FirstName = "Super",
                        LastName = "Admin",
                        AddressLine1 = "1 Admin Street",
                        AddressLine2 = "Suite 101",
                        City = "Cairo",
                        PostalCode = "12345",
                        Country = "Egypt",
                        PhoneNumber = "+201234567890",
                        IsDefault = true,
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow
                    }
                };

                await _userManager.CreateAsync(superAdmin, "Admin@123456");
            }
        }

        private async Task SeedDemoUsersAsync()
        {
            var demoUsers = new[]
            {
                new { Email="customer1@example.com", Username="customer1", Password="Customer@123", Role="Customer", FirstName="John", LastName="Doe"},
                new { Email="customer2@example.com", Username="customer2", Password="Customer@123", Role="Customer", FirstName="Jane", LastName="Smith"},
                new { Email="vendor@example.com", Username="vendor1", Password="Vendor@123", Role="Vendor", FirstName="Bob", LastName="Wilson"},
                new { Email="support@example.com", Username="support1", Password="Support@123", Role="Support", FirstName="Alice", LastName="Johnson"}
            };

            int index = 1;
            foreach (var u in demoUsers)
            {
                if (await _userManager.FindByEmailAsync(u.Email) == null)
                {
                    var user = new ApplicationUser
                    {
                        UserName = u.Username,
                        Email = u.Email,
                        EmailConfirmed = true,
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        MobileNumber = $"+2010000000{index}",
                        PreferredLanguage = "en",
                        TimeZone = "UTC",
                        IsActive = true,
                        CreatedAt = DateTime.UtcNow,
                        Bio = "Demo user",
                        AlternativeEmail = $"demo{index}@example.com",
                        ProfilePictureUrl = "https://example.com/default-profile.png"
                    };

                    await _userManager.CreateAsync(user, u.Password);
                    await _userManager.AddToRoleAsync(user, u.Role);

                    index++;
                }
            }
        }
        #endregion
    }
}

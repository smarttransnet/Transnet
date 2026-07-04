using Infrastructure.Database;
using Domain.Trips;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Web.Api.Extensions;

public static class MigrationExtensions
{
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using IServiceScope scope = app.ApplicationServices.CreateScope();

        using ApplicationDbContext dbContext =
            scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        dbContext.Database.Migrate();

        dbContext.SeedTripCategories();

        var passwordHasher = scope.ServiceProvider.GetRequiredService<Application.Abstractions.Authentication.IPasswordHasher>();

        if (!dbContext.Users.Any(u => u.Email == "admin@transnet.com"))
        {
            var user = new Domain.Users.User
            {
                Id = Guid.NewGuid(),
                Email = "admin@transnet.com",
                FirstName = "System",
                LastName = "Administrator",
                PasswordHash = passwordHasher.Hash("admin")
            };
            dbContext.Users.Add(user);
        }

        if (!dbContext.Users.Any(u => u.Email == "test@test.com"))
        {
            var user = new Domain.Users.User
            {
                Id = Guid.NewGuid(),
                Email = "test@test.com",
                FirstName = "Test",
                LastName = "User",
                PasswordHash = passwordHasher.Hash("12121212")
            };
            dbContext.Users.Add(user);
        }

        dbContext.SaveChanges();
    }

    private static void SeedTripCategories(this ApplicationDbContext dbContext)
    {
        if (dbContext.TripCategories.Any())
        {
            return;
        }

        var seedData = new (string Category, string UOM)[]
        {
            ("Sewage Water", "GL"),
            ("Sewage Water", "KL"),
            ("Concrete Works", "CBM"),
            ("Concrete Works", "BAG"),
            ("Road Works", "TON"),
            ("Road Works", "CBM"),
            ("Earth Works", "LOAD"),
            ("Earth Works", "CBM"),
            ("Plumbing", "M"),
            ("Plumbing", "PCS"),
            ("Electrical", "M"),
            ("Electrical", "EA"),
            ("Steel Works", "TON"),
            ("Steel Works", "BDL"),
            ("Fuel Supply", "L"),
            ("Fuel Supply", "GL"),
            ("Equipment Rental", "HR"),
            ("Equipment Rental", "DAY"),
            ("Transportation", "TRIP"),
            ("Transportation", "LOAD")
        };

        var categories = new Dictionary<string, TripCategory>();
        var uoms = new Dictionary<string, Uom>();

        var systemUserId = Guid.Parse("00000000-0000-0000-0000-000000000001");
        var now = DateTime.UtcNow;

        foreach (var item in seedData)
        {
            // 1. Get or Create Category
            if (!categories.TryGetValue(item.Category, out var category))
            {
                category = new TripCategory
                {
                    Id = Guid.NewGuid(),
                    CategoryName = item.Category,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = systemUserId
                };
                categories[item.Category] = category;
                dbContext.TripCategories.Add(category);
            }



            // 3. Get or Create UOM
            if (!uoms.TryGetValue(item.UOM, out var uom))
            {
                uom = dbContext.Uoms.FirstOrDefault(u => u.UOMCode == item.UOM);
                if (uom == null)
                {
                    uom = new Uom
                    {
                        Id = Guid.NewGuid(),
                        UOMCode = item.UOM,
                        Description = item.UOM + " Unit",
                        IsActive = true
                    };
                    dbContext.Uoms.Add(uom);
                }
                uoms[item.UOM] = uom;
            }

            // 4. Create TripCategoryMaterial mapping
            var mapping = new TripCategoryMaterial
            {
                Id = Guid.NewGuid(),
                TripCategoryId = category.Id,

                UOMId = uom.Id,
                IsActive = true,
                CreatedDate = now,
                CreatedBy = systemUserId
            };
            dbContext.TripCategoryMaterials.Add(mapping);
        }

        dbContext.SaveChanges();
    }
}

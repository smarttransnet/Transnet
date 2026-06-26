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

        var seedData = new (string Category, string Material, string UOM)[]
        {
            ("Sewage Water", "Treated Water", "GL"),
            ("Sewage Water", "Treated Water", "KL"),
            ("Concrete Works", "Ready Mix Concrete", "CBM"),
            ("Concrete Works", "Cement", "BAG"),
            ("Road Works", "Aggregate", "TON"),
            ("Road Works", "Crusher Run", "CBM"),
            ("Earth Works", "Fill Material", "LOAD"),
            ("Earth Works", "Excavation", "CBM"),
            ("Plumbing", "PVC Pipe", "M"),
            ("Plumbing", "Pipe Fittings", "PCS"),
            ("Electrical", "Cable", "M"),
            ("Electrical", "Junction Box", "EA"),
            ("Steel Works", "Reinforcement Bars", "TON"),
            ("Steel Works", "Rebar", "BDL"),
            ("Fuel Supply", "Diesel", "L"),
            ("Fuel Supply", "Diesel", "GL"),
            ("Equipment Rental", "Excavator", "HR"),
            ("Equipment Rental", "Excavator", "DAY"),
            ("Transportation", "Water Delivery", "TRIP"),
            ("Transportation", "Sand Delivery", "LOAD")
        };

        var categories = new Dictionary<string, TripCategory>();
        var materials = new Dictionary<(string Category, string Material), Material>();
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

            // 2. Get or Create Material
            var materialKey = (item.Category, item.Material);
            if (!materials.TryGetValue(materialKey, out var material))
            {
                material = new Material
                {
                    Id = Guid.NewGuid(),
                    TripCategoryId = category.Id,
                    MaterialName = item.Material,
                    IsActive = true,
                    CreatedDate = now,
                    CreatedBy = systemUserId
                };
                materials[materialKey] = material;
                dbContext.Materials.Add(material);
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
                MaterialId = material.Id,
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

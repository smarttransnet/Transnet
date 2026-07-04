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
}

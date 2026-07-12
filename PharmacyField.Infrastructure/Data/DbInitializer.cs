using Microsoft.EntityFrameworkCore;
using PharmacyField.Core.Entities;
using PharmacyField.Infrastructure.Utils;

namespace PharmacyField.Infrastructure.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context)
        {
            // Ensure migrations are applied
            await context.Database.MigrateAsync();

            // Seed default admin user if none exists
            bool adminExists = await context.Users.AnyAsync(u => u.Role == "Admin");
            if (!adminExists)
            {
                var admin = new User
                {
                    EmployeeCode = "EMP001",
                    FullName = "System Administrator",
                    Email = "admin@gmail.com",
                    PhoneNumber = "8379043350",
                    PasswordHash = PasswordHelper.HashPassword("Admin@123"),
                    Role = "Admin",
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await context.Users.AddAsync(admin);
                await context.SaveChangesAsync();
            }
        }
    }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ArteEva.Models;

namespace ArtEva.Data.Data_Seeder
{
    public static class DataSeeder
    {
        public static async Task SeedSuperAdminAsync(IServiceProvider service)
        {
            var userManager = service.GetRequiredService<UserManager<User>>();
            var roleManager = service.GetRequiredService<RoleManager<Role>>();

            string superAdminEmail = "admin@admin.com";
            string superAdminUsername = "superadmin"; // <-- login username
            string superAdminPassword = "Admin@123*";

            // Create roles if they don't exist
            if (!await roleManager.RoleExistsAsync("SuperAdmin"))
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "SuperAdmin",
                    NormalizedName = "SUPERADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Description = "Super administrator with full system access",
                    CreatedAt = DateTime.UtcNow
                });
            }

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new Role
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = Guid.NewGuid().ToString(),
                    Description = "Administrator with full system access",
                    CreatedAt = DateTime.UtcNow
                });
            }

            // Check if super admin already exists
            var superAdmin = await userManager.FindByNameAsync(superAdminUsername);

            if (superAdmin == null)
            {
                var newSuperAdmin = new User()
                {
                    UserName = superAdminUsername, // <-- username for login
                    Email = superAdminEmail,
                    EmailConfirmed = true,
                    IsActive = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(newSuperAdmin, superAdminPassword);

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(newSuperAdmin, "SuperAdmin");
                }
                else
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception($"Failed to create SuperAdmin user: {errors}");
                }
            }
        }
    }

}

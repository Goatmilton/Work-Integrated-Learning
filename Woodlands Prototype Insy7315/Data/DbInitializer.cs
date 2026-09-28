using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Woodlands_Prototype_Insy7315.Models;

namespace Woodlands_Prototype_Insy7315.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(); 

            await context.Database.MigrateAsync();

            string[] roleNames = { "Admin", "Manager", "Sales", "Customer" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            if (!await context.Users.AnyAsync(u => u.Email == "admin@woodlands.co.za"))
            {
                var adminUser = new ApplicationUser 
                {
                    UserName = "admin@woodlands.co.za",
                    Email = "admin@woodlands.co.za",
                    EmailConfirmed = true
                };

                var createAdmin = await userManager.CreateAsync(adminUser, "WoodlandsAdmin@2026!");
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            
            if (!await context.ProductCategories.AnyAsync())
            {
                await context.ProductCategories.AddRangeAsync(WoodLinkData.Categories);
                await context.SaveChangesAsync();
            }

            if (!await context.Products.AnyAsync())
            {
                await context.Products.AddRangeAsync(WoodLinkData.Products);
                await context.SaveChangesAsync();
            }

            if (!await context.HeroSlides.AnyAsync())
            {
                await context.HeroSlides.AddRangeAsync(WoodLinkData.HeroSlides);
                await context.SaveChangesAsync();
            }

        }
    }
}

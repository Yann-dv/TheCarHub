using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace theCarHub.Data;

public class UserSeed
{
    public static async Task SeedUsersAsync(IApplicationBuilder applicationBuilder)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
            var rolesList = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
            
            await context.Database.EnsureCreatedAsync();

            if (!rolesList.Roles.Any())
            {
                await rolesList.CreateAsync(new IdentityRole { Name = "SuperAdmin", NormalizedName = "SUPERADMIN" });
                await rolesList.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                await rolesList.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
            }
            
            if (!context.Users.Any())
            {
                string pwd3 = "Johndoe+123";
                string pwd2 = "Eddie+123";

                var hasher = new PasswordHasher<IdentityUser>();

                // Cars to seed if existing but Empty db
                var usersToSeed = new List<ApplicationUser>()
                {
                    new()
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        UserName = "John",
                        NormalizedUserName = "JOHN",
                        Email = "john@doe.com",
                        NormalizedEmail = "JOHN@DOE.COM",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, pwd3)
                    },
                    new()
                    {
                        Id = "4151a77b-11f2-41e5-90bc-c07a6ed3e57c",
                        FirstName = "Eddie",
                        LastName = "The Owner",
                        UserName = "Eddie",
                        NormalizedUserName = "EDDIE",
                        Email = "eddie@owner.com",
                        NormalizedEmail = "EDDIE@OWNER.COM",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, pwd2)
                    }
                };

                foreach (var t in usersToSeed)
                {
                    context.Users.Add(t);
                }
            }

            if (!rolesList.Roles.Any())
            {
                var hasher = new PasswordHasher<IdentityUser>();

                await rolesList.CreateAsync(new IdentityRole { Name = "SuperAdmin", NormalizedName = "SUPERADMIN" });
                await rolesList.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                await rolesList.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
            }
            await context.SaveChangesAsync();
        }
    }
    
    
    public static async Task SeedSuperAdminAsync(IApplicationBuilder applicationBuilder)
        //UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
        {
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            var roleManager = serviceScope.ServiceProvider.GetService<RoleManager<IdentityRole>>();
           
            //Seed super admin
            var superAdmin = new ApplicationUser()
            {
                Id = "0b9c2ad2-8e49-44ba-b1b5-278b76c33804",
                FirstName = "Administrator",
                LastName = "Super",
                UserName = "SuperAdmin",
                NormalizedUserName = "SUPERADMIN",
                Email = "administrator@admin.com",
                NormalizedEmail = "ADMINISTRATOR@ADMIN.COM",
                EmailConfirmed = true,
            };
            if (userManager.Users.All(u => u.Id != superAdmin.Id))
            {
                var user = await userManager.FindByEmailAsync(superAdmin.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(superAdmin, "Admin+123");
                    await userManager.AddToRoleAsync(superAdmin, "SuperAdmin");
                }
            }
        }
    }
}
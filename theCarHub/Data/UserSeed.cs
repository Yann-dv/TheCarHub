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
            //var userRolesList = serviceScope.ServiceProvider.GetService<RoleManager<IdentityUserRole<string>>>();
            var userManager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();
            await context.Database.EnsureCreatedAsync();

            if (!rolesList.Roles.Any())
            {
                await rolesList.CreateAsync(new IdentityRole { Name = "SuperAdmin", NormalizedName = "SUPERADMIN" });
                await rolesList.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                await rolesList.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
            }
            
            if (!context.Users.Any())
            {
                string pwd1 = "Admin+123";
                string pwd3 = "Johndoe+123";
                string pwd2 = "Eddie+123";

                var hasher = new PasswordHasher<IdentityUser>();

                // Cars to seed if existing but Empty db
                var usersToSeed = new List<ApplicationUser>()
                {
                    new()
                    {
                        Id = "0b9c2ad2-8e49-44ba-b1b5-278b76c33804",
                        FirstName = "Administrator",
                        LastName = "Super",
                        UserName = "SuperAdmin",
                        NormalizedUserName = "SUPERADMIN",
                        Email = "administrator@admin.com",
                        NormalizedEmail = "ADMINISTRATOR@ADMIN.COM",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, pwd1)
                    },
                    new()
                    {
                        FirstName = "John",
                        LastName = "Doe",
                        UserName = "John",
                        NormalizedUserName = "JOHN",
                        Email = "john@doe.com",
                        NormalizedEmail = "JOHN@DOE.COM",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, pwd2)
                    },
                    new()
                    {
                        FirstName = "Eddie",
                        LastName = "The Owner",
                        UserName = "Eddie",
                        NormalizedUserName = "EDDIE",
                        Email = "eddie@owner.com",
                        NormalizedEmail = "EDDIE@OWNER.COM",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, pwd3)
                    }
                };

                for (int i = 0; i < usersToSeed.Count; i++)
                {
                    //TODO complete to add superadmin role
                    context.Users.Add(usersToSeed[i]); 
                    if (usersToSeed[i].Email == "administrator@admin.com")
                    {
                        await userManager.AddToRoleAsync(usersToSeed[i], "SuperAdmin");
                    }
                }
            }

            if (!rolesList.Roles.Any())
            {
                var hasher = new PasswordHasher<IdentityUser>();

                await rolesList.CreateAsync(new IdentityRole
                {
                    Id = "8a498787-dbe9-4dc7-b70a-ce424e80b87d", Name = "SuperAdmin", NormalizedName = "SUPERADMIN"
                });
                await rolesList.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
                await rolesList.CreateAsync(new IdentityRole { Name = "User", NormalizedName = "USER" });
            }
            
            /*await userManager.AddToRoleAsync(
                await userManager.CreateAsync(
                    new ApplicationUser()
                    {
                        Id = "0b9c2ad2-8e49-44ba-b1b5-278b76c33804",
                        FirstName = "Administrator",
                        LastName = "Super",
                        UserName = "SuperAdmin",
                        NormalizedUserName = "SUPERADMIN",
                        Email = "administrator@admin.com",
                        NormalizedEmail = "ADMINISTRATOR@ADMIN.COM",
                        EmailConfirmed = true,
                        PasswordHash = hasher.HashPassword(null, "Admin+123")
                    })
                , "SuperAdmin");*/
                /*await userRolesList.Add.CreateAsync(new IdentityUserRole<string>
                {
                    UserId = "0b9c2ad2-8e49-44ba-b1b5-278b76c33804",
                    RoleId = "8a498787-dbe9-4dc7-b70a-ce424e80b87d"
                });*/
            
            
            await context.SaveChangesAsync();
        }
    }
}
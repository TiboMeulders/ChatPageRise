using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Domain.Products;
using Rise.Domain.Projects;
using Rise.Domain.Account;

namespace Rise.Persistence;
/// <summary>
/// Seeds the database
/// </summary>
/// <param name="dbContext"></param>
/// <param name="roleManager"></param>
/// <param name="userManager"></param>
public class DbSeeder(ApplicationDbContext dbContext, RoleManager<IdentityRole> roleManager, UserManager<IdentityUser> userManager)
{
    const string PasswordDefault = "A1b2C3!";
    public async Task SeedAsync()
    {
        await FacilitiesAsync();
        await RolesAsync();
        await UsersAsync();
        await AccountAsync();
    }

    private async Task FacilitiesAsync()
    {
        if (dbContext.Facilities.Any())
            return;

        dbContext.Facilities.Add(new Domain.Facility.Facility
        {
            Name = "TestFacility"
        });

        await dbContext.SaveChangesAsync();
    }

    private async Task RolesAsync()
    {
        try
        {
            if (dbContext.Roles.Any())
                return;
        }
        catch
        {
            // Table doesn't exist yet, continue with creation
        }

        await roleManager.CreateAsync(new IdentityRole("Administrator"));
        await roleManager.CreateAsync(new IdentityRole("Educator"));
        await roleManager.CreateAsync(new IdentityRole("User"));
    }

    private async Task UsersAsync()
    {
        if (dbContext.Users.Any())
            return;

        await dbContext.Roles.ToListAsync();

        var admin = new IdentityUser
        {
            UserName = "admin@example.com",
            Email = "admin@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(admin, PasswordDefault);
        
        var educator = new IdentityUser
        {
            UserName = "educator@example.com",
            Email = "educator@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(educator, PasswordDefault);

        var user = new IdentityUser
        {
            UserName = "user@example.com",
            Email = "user@example.com",
            EmailConfirmed = true,
        };
        await userManager.CreateAsync(user, PasswordDefault);

        await userManager.AddToRoleAsync(admin, "Administrator");
        await userManager.AddToRoleAsync(educator, "Educator");

        await dbContext.SaveChangesAsync();
    }

    private async Task AccountAsync()
    {
        var users = await dbContext.Users.ToListAsync();
        
        var facility = await dbContext.Facilities.Where(x => x.Name == "TestFacility").FirstOrDefaultAsync(); 
        dbContext.Account.Add(new Domain.Account.Account
        {
            UserId = users[0].Id.ToString(),
            FirstName = "Ad",
            LastName = "Min",
            BirthDate = new DateOnly(1980, 1, 1),
            Pronouns = "they/them",
            Bio = "Lorem ipsum dolor sit amet",
            Facility = facility,
        });
        dbContext.Account.Add(new Domain.Account.Account
        {
            UserId = users[1].Id.ToString(),
            FirstName = "Ed",
            LastName = "Ucator",
            BirthDate = new DateOnly(2001, 1, 1),
            Pronouns = "they/them",
            Bio = "Lorem ipsum dolor sit amet",
            Facility = facility,
        });
        dbContext.Account.Add(new Domain.Account.Account
        {
            UserId = users[2].Id.ToString(),
            FirstName = "U",
            LastName = "Ser",
            BirthDate = new DateOnly(2008, 1, 1),
            Pronouns = "they/them",
            Bio = "Lorem ipsum dolor sit amet",
            Facility = facility,
        });
        await dbContext.SaveChangesAsync();
    }
}
using Microsoft.AspNetCore.Identity;
using Rise.Domain.Account;
using Rise.Persistence;
using Rise.Shared.Identity.Accounts;

namespace Rise.Server.Endpoints.Identity.Accounts;

/// <summary>
/// Register a new user,
/// See https://fast-endpoints.com/
/// </summary>
/// <param name="userManager"></param>
/// <param name="userStore"></param>
public class Register(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IUserStore<IdentityUser> userStore) : Endpoint<AccountRequest.Register, Result>
{
    public override void Configure()
    {
        Post("/api/identity/accounts/register");
        AllowAnonymous(); // Open for all at the moment, but you can restrict it to only admins.
                          // Roles(AppRoles.Administrator);
    }

    public override async Task<Result> ExecuteAsync(AccountRequest.Register req, CancellationToken ctx)
    {
        if (!userManager.SupportsUserEmail)
        {
            return Result.CriticalError("Requires a user store with email support.");
        }
        
        var emailStore = (IUserEmailStore<IdentityUser>)userStore;
        var user = new IdentityUser();
        await userStore.SetUserNameAsync(user, req.Email, CancellationToken.None);
        await emailStore.SetEmailAsync(user, req.Email, CancellationToken.None);
        var result = await userManager.CreateAsync(user, req.Password!);
        
        if (!result.Succeeded)
        {
            return Result.Error(result.Errors.First().Description);
        }
        
        
        // You can do more stuff when injecting a DbContext and create user stuff for example:
        // dbContext.Technicians.Add(new Technician("Fname", "Lname", user.Id));
        // or assinging a specific role etc using the RoleManager<IdentityUser> (inject it in the primary constructor).
        var facility = dbContext.Facilities.Find(req.FacilityId);
        if (facility is null)
        {
            return Result.Error("Facility not found");
        }
        
        await dbContext.Account.AddAsync(new Account(){
            UserId = user.Id,
            FirstName = req.FirstName,
            LastName = req.LastName, 
            BirthDate = req.BirthDate.GetValueOrDefault(), //TODO - Mag dit?
            Facility = facility,
        });
        
        await dbContext.SaveChangesAsync();
        
        // You can send a confirmation email by using a SMTP server or anything in the like. 
        // await SendConfirmationEmailAsync(user, userManager, context, email); or do something that matters

        return Result.Success();
    }
    
}
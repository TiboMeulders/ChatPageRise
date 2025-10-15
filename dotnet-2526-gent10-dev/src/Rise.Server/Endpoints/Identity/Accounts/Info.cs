using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;
using Rise.Shared.Identity.Accounts;

namespace Rise.Server.Endpoints.Identity.Accounts;

/// <summary>
/// Get the logged in user info, Roles, Claims, etc.
/// See https://fast-endpoints.com/
/// </summary>
/// <param name="userManager"></param>
public class Info(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext) : EndpointWithoutRequest<Result<AccountResponse.Info>>
{
    public override void Configure()
    {
        Get("/api/identity/accounts/info");
    }

    public override async Task<Result<AccountResponse.Info>> ExecuteAsync(CancellationToken ct)
    {
        if (await userManager.GetUserAsync(HttpContext.User) is not { } user)
        {
            return Result.NotFound();
        }
        
        var account = await dbContext.Account
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.UserId == user.Id, ct);

        return Result.Success(await CreateInfoResponseAsync(user, HttpContext.User, account?.Id ?? 0));       
    }
    
    private async Task<AccountResponse.Info> CreateInfoResponseAsync(IdentityUser user,ClaimsPrincipal claimsPrincipal, int accountId)
    {
        return new()
        {
            AccountId = accountId,
            Email = user.Email!,
            IsEmailConfirmed = await userManager.IsEmailConfirmedAsync(user),
            Claims = claimsPrincipal.Claims.ToDictionary(c => c.Type, c => c.Value),
            Roles = claimsPrincipal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToList()
        };
    }
}

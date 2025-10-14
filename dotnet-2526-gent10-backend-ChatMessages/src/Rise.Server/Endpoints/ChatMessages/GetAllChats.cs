using Rise.Shared.ChatMessages;

namespace Rise.Server.Endpoints.ChatMessages;

using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rise.Persistence;




public class GetAllChats(UserManager<IdentityUser> userManager, ApplicationDbContext dbContext) 
    : EndpointWithoutRequest<Result<List<ChatContactDto>>>
{
    public override void Configure()
    {
        Get("/api/chats");
        // Alleen ingelogde users kunnen chats ophalen
    }

    public override async Task<Result<List<ChatContactDto>>> ExecuteAsync(CancellationToken ct)
    {
        // Haal de huidige user op
        var currentUserId = User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(currentUserId))
        {
            return Result.Unauthorized("User not authenticated");
        }

        // Haal alle users op BEHALVE de huidige user
        var contacts = await (
            from user in userManager.Users
            join account in dbContext.Account on user.Id equals account.UserId
            where user.Id != currentUserId
            select new ChatContactDto
            {
                Id = user.Id,
                FirstName = account.FirstName,
                LastName = account.LastName,
                Email = user.Email ?? "",
                // Je kan hier meer velden toevoegen zoals ProfilePictureUrl etc.
            }
        ).ToListAsync(ct);

        return Result.Success(contacts);
    }
}


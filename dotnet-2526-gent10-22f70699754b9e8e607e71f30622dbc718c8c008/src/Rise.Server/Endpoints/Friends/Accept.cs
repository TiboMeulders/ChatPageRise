using FastEndpoints;
using Rise.Services;
using Rise.Shared.DTOs;
using Rise.Shared.Identity;
using Rise.Shared.Common;

namespace Rise.Server.Endpoints.Friends;

public class Accept : Endpoint<FriendAcceptDto, object>
{
    private readonly IFriendService _friendService;

    public Accept(IFriendService friendService) => _friendService = friendService;

    public override void Configure()
    {
        Post("/api/friends/accept");
        Roles(AppRoles.User, AppRoles.Administrator);
    }

    public override async Task<object> ExecuteAsync(FriendAcceptDto req, CancellationToken ct)
    {
        // Get authenticated user ID from claims (fallback 1 for testing)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        int currentUserId = int.TryParse(userIdClaim, out var id) ? id : 1;

        // Call the service to accept the friend request
        var result = await _friendService.AcceptRequestAsync(currentUserId, req.FromUserId);

        // Return the result directly; FastEndpoints will serialize it as JSON
        return result;
    }
}
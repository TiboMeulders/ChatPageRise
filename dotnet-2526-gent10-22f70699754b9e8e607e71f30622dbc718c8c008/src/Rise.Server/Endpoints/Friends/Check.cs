using FastEndpoints;
using Rise.Services;
using Rise.Shared.DTOs;
using Rise.Shared.Identity;

namespace Rise.Server.Endpoints.Friends;

public class FriendCheckQuery
{
    public int user_id { get; set; }
}

public class Check : Endpoint<FriendCheckQuery, FriendCheckDto>
{
    private readonly IFriendService _friendService;

    public Check(IFriendService friendService) => _friendService = friendService;

    public override void Configure()
    {
        Get("/api/friends/check");
        Roles(AppRoles.User, AppRoles.Administrator);
        AllowAnonymous(); // remove if you want authentication required
    }

    public override async Task<FriendCheckDto> ExecuteAsync(FriendCheckQuery req, CancellationToken ct)
    {
        // Get the authenticated user's ID from claims (fallback to 1 for testing)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        int currentUserId = int.TryParse(userIdClaim, out var id) ? id : 1;

        // Call the service to check friend status
        var check = await _friendService.CheckFriendStatusAsync(currentUserId, req.user_id);

        // Return the result directly; FastEndpoints will serialize it as JSON
        return check;
    }
}
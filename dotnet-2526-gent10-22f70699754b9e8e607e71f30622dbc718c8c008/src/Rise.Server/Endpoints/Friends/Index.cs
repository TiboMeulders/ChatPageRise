using FastEndpoints;
using Rise.Services;
using Rise.Shared.DTOs;
using Rise.Shared.Identity;

namespace Rise.Server.Endpoints.Friends;

/// <summary>
/// List all friends of the current user.
/// </summary>
public class Index : EndpointWithoutRequest<FriendResponse.Index>
{
    private readonly IFriendService _friendService;

    public Index(IFriendService friendService) => _friendService = friendService;

    public override void Configure()
    {
        Get("/api/friends");
        Roles(AppRoles.User, AppRoles.Administrator);
    }

    public override async Task<FriendResponse.Index> HandleAsync(CancellationToken ct)
    {
        // Get the authenticated user's ID from claims (fallback to 1 for testing)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        int userId = int.TryParse(userIdClaim, out var parsedId) ? parsedId : 1;

        // Get friends from the service
        var friends = await _friendService.GetFriendsAsync(userId);

        // Return the response directly
        return new FriendResponse.Index(friends);
    }
}
using FastEndpoints;
using Rise.Services;
using Rise.Shared.DTOs;
using Rise.Shared.Identity;

namespace Rise.Server.Endpoints.Friends;

public class Request : Endpoint<FriendRequestDto, object>
{
    private readonly IFriendService _friendService;

    public Request(IFriendService friendService) => _friendService = friendService;

    public override void Configure()
    {
        Post("/api/friends/request");
        Roles(AppRoles.User, AppRoles.Administrator);
    }

    public override async Task<object> ExecuteAsync(FriendRequestDto req, CancellationToken ct)
    {
        // Get authenticated user ID from claims (fallback to 1 for testing)
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        int userId = int.TryParse(userIdClaim, out var id) ? id : 1;

        // Call the service to send a friend request
        var result = await _friendService.SendRequestAsync(userId, req.ToUserId);

        // Return result directly; FastEndpoints will serialize it as JSON
        return result;
    }
}
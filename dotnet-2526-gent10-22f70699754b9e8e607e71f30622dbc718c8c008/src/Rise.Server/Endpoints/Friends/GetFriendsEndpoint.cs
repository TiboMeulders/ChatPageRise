using FastEndpoints;
using Rise.Services;
using Rise.Shared.DTOs;
using Rise.Shared.Identity;

public class GetFriendsEndpoint : EndpointWithoutRequest<IEnumerable<FriendDto>>
{
    private readonly IFriendService _friendService;

    public GetFriendsEndpoint(IFriendService friendService) => _friendService = friendService;

    public override void Configure()
    {
        Get("/friends");
        // AllowAnonymous();
    }

    public override async Task<IEnumerable<FriendDto>> HandleAsync(CancellationToken ct)
    {
        var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")?.Value;
        int userId = int.TryParse(userIdClaim, out var id) ? id : 1;

        var friends = await _friendService.GetFriendsAsync(userId);
        return friends;
    }
}

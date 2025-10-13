namespace Rise.Shared.DTOs;

public class FriendRequestDto
{
    public int ToUserId { get; set; }
}

public class FriendAcceptDto
{
    public int FromUserId { get; set; }
}

public class FriendDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public bool Accepted { get; set; }
}

public class FriendCheckDto
{
    public bool IsFriend { get; set; }
    public bool Accepted { get; set; }
    public bool Blocked { get; set; }
}

public static class FriendResponse
{
    public record Index(IEnumerable<FriendDto> Friends);
    public record Status(bool IsFriend);
}

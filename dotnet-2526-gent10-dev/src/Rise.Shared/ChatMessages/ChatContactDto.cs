namespace Rise.Shared.ChatMessages;

public class ChatContactDto
{
    public required string Id { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public string FullName => $"{FirstName} {LastName}";
}
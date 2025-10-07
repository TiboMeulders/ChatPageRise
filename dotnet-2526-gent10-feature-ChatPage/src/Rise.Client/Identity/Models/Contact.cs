namespace Rise.Client.Identity.Models;

public class Contact
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public bool IsOnline { get; set; }
    public bool HasSosAlert { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? LastMessage { get; set; }
    public DateTime? LastMessageTime { get; set; }
}
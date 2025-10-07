namespace Rise.Client.Identity.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string Text { get; set; } = string.Empty;
    public bool IsFromCurrentUser { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsCensored { get; set; }
    public bool IsRead { get; set; }
    public int SenderId { get; set; }
    public int ReceiverId { get; set; }
}
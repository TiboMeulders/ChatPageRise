namespace Rise.Client.Models;

public class ChatMessage
{
    public int Id { get; set; }
    public string Payload { get; set; } = string.Empty;
    public bool IsFromCurrentUser { get; set; }
    public DateTime Timestamp { get; set; }
    public bool IsCensored { get; set; }
    public bool IsRead { get; set; }
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
}
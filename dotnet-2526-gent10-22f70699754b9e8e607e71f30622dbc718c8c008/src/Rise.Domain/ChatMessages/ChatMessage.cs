namespace Rise.Domain.ChatMessages;

public class ChatMessage : MongoEntity
{
    public ChatMessage(string payload, string senderId, string receiverId)
    {
        Payload = Guard.Against.NullOrWhiteSpace(payload);
        SenderId = senderId;
        ReceiverId = receiverId;
        SentAt = DateTime.UtcNow;

        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
        IsDeleted = false;
    }

    public string Payload { get; set; }
    public DateTime SentAt { get; set; }
    public string Status { get; set; } = "Sent";
    public string SenderId { get; set; }
    public string ReceiverId { get; set; }
}
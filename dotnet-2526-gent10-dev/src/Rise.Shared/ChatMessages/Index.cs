using MongoDB.Bson;

namespace Rise.Shared.ChatMessages;

public static partial class ChatMessageResponse
{
    public class Index
    {
        public ObjectId Id { get; set; }
        public string Payload { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public bool IsRead { get; set; }
        public required string SenderId { get; set; }
        public required string ReceiverId { get; set; }
    }
}

public static partial class ChatMessageRequest
{
    public class Index
    {
        public string ReceiverId { get; set; } = string.Empty;
    }
}
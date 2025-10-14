using MongoDB.Bson;

namespace Rise.Shared.ChatMessages;

public static partial class ChatMessageRequest
{
    public class Create
    {
        public string Payload { get; set; } = string.Empty;
        public DateTime SentAt { get; set; } = DateTime.Now;
        public string ReceiverId { get; set; } = string.Empty;
    }
}

public partial class ChatMessageResponse
{
    public class Create
    {
        public required ObjectId Id { get; set; }
    }
}
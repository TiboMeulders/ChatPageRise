using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Rise.Domain.ChatMessages;

namespace Rise.Persistence.Configurations.ChatMessages;

public class ChatMessageConfiguration
{
    public static void Configure(IMongoDatabase db)
    {
        var chatCollection = db.GetCollection<ChatMessage>("ChatMessages");
        
        BsonClassMap.RegisterClassMap<ChatMessage>(cm =>
        {
            cm.AutoMap();
            cm.MapIdProperty(c => c.Id);
            cm.MapProperty(c => c.Payload)
                .SetElementName("payload");
            cm.MapProperty(c => c.SentAt)
                .SetElementName("sentAt");
            cm.MapProperty(c => c.Status)
                .SetElementName("status");
            cm.MapProperty(c => c.SenderId)
                .SetElementName("senderId");
            cm.MapProperty(c => c.ReceiverId)
                .SetElementName("receiverId");
        });
    }
}
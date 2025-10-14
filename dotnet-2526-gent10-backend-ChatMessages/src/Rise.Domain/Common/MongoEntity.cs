using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Rise.Domain.Common;

public class MongoEntity
{
    public MongoEntity()
    {
        Id = ObjectId.GenerateNewId();
    }

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public ObjectId Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}
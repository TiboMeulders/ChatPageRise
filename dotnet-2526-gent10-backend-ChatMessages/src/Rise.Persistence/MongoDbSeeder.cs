using MongoDB.Driver;
using Rise.Domain.ChatMessages;

namespace Rise.Persistence;

public class MongoDbSeeder
{
    private readonly IMongoDatabase _database;

    public MongoDbSeeder(IMongoDatabase database)
    {
        _database = database;
    }

    public async Task SeedAsync()
    {
        var chatCollection = _database.GetCollection<ChatMessage>("ChatMessages");

        await chatCollection.DeleteManyAsync(_ => true);

        var messages = new List<ChatMessage>
        {
            new ChatMessage("Hello, welcome to RISE!", "b1d4bcf6-ac94-4878-8dac-54317c3700e1", "2"),
            new ChatMessage("Hey, how are you?", "2", "1"),
            new ChatMessage("Don't forget the meeting tomorrow.", "b1d4bcf6-ac94-4878-8dac-54317c3700e1", "2")
        };

        await chatCollection.InsertManyAsync(messages);
    }
}

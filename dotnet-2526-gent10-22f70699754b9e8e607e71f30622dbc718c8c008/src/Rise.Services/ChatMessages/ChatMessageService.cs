using MongoDB.Driver;
using Rise.Domain.ChatMessages;
using Rise.Services.Identity;
using Rise.Shared.ChatMessages;

namespace Rise.Services.ChatMessages;

public class ChatMessageService : IChatMessageService
{
    private readonly IMongoCollection<ChatMessage> _collection;

    public ChatMessageService(IMongoDatabase database, ISessionContextProvider identityContextProvider)
    {
        _collection = database.GetCollection<ChatMessage>("ChatMessages");
    }


    public async Task<Result<List<ChatMessageResponse.Index>>> GetAllBySenderIdAndReceiverIdAsync(ChatMessageRequest.Index chatMessage, CancellationToken cancellationToken)
    {
        var messages = await _collection
            .Find(m => m.ReceiverId.Equals(chatMessage.ReceiverId) &&  m.SenderId.Equals(chatMessage.SenderId) 
                       || m.ReceiverId.Equals(chatMessage.ReceiverId) && m.SenderId.Equals(chatMessage.SenderId))
            .Project(m => new ChatMessageResponse.Index
            {
                Payload = m.Payload,
                SentAt = m.SentAt,
                IsRead = m.IsDeleted,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId
            })
            .ToListAsync(cancellationToken);

        return Result.Success(messages);
    }

    

    public async Task<Result<List<ChatMessageResponse.Index>>> GetAllByUserIdAsync(string id, CancellationToken cancellationToken)
    {
        return await _collection.Find(m => m.SenderId.Equals(id) || m.ReceiverId.Equals(id)).Project(m => new ChatMessageResponse.Index
            {
                Payload = m.Payload,
                SentAt = m.SentAt,
                IsRead = m.IsDeleted,
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<Result<ChatMessageResponse.Create>> CreateAsync(ChatMessageRequest.Create request,
        CancellationToken cancellationToken = default)
    {
        var chatMessage = new ChatMessage(request.Payload, request.SenderId, request.ReceiverId);
        
        await _collection.InsertOneAsync(chatMessage, cancellationToken: cancellationToken);

        return Result.Created(new ChatMessageResponse.Create
        {
            Id = chatMessage.Id
        });
    }
}
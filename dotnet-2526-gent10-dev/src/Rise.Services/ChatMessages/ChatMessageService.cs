using MongoDB.Driver;
using Rise.Domain.ChatMessages;
using Rise.Services.Identity;
using Rise.Shared.ChatMessages;
using Rise.Shared.Identity;

namespace Rise.Services.ChatMessages;

public class ChatMessageService : IChatMessageService
{
    private readonly IMongoCollection<ChatMessage> _collection;
    private readonly ISessionContextProvider _sessionContextProvider;

    public ChatMessageService(IMongoDatabase database, ISessionContextProvider sessionContextProvider)
    {
        _collection = database.GetCollection<ChatMessage>("ChatMessages");
        _sessionContextProvider = sessionContextProvider;
    }


    public async Task<Result<List<ChatMessageResponse.Index>>> GetAllByLoggedInUserAndReceiverIdAsync(ChatMessageRequest.Index chatMessage, CancellationToken cancellationToken)
    {
        var currentUserId = _sessionContextProvider.User?.GetUserId();

        var accountId = 2;
        
        var messages = await _collection
            .Find(m => m.ReceiverId.Equals(chatMessage.ReceiverId) &&  m.SenderId.Equals(currentUserId) 
                       || m.ReceiverId.Equals(currentUserId) && m.SenderId.Equals(chatMessage.ReceiverId))
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

    

    public async Task<Result<List<ChatMessageResponse.Index>>> GetAllByLoggedInUserAsync(CancellationToken cancellationToken)
    {
        var currentUserId = _sessionContextProvider.User?.GetUserId();
        return await _collection.Find(m => m.SenderId.Equals(currentUserId) || m.ReceiverId.Equals(currentUserId)).Project(m => new ChatMessageResponse.Index
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
        var currentUserId = _sessionContextProvider.User?.GetUserId();
        
        var chatMessage = new ChatMessage(request.Payload, currentUserId, request.ReceiverId);
        
        await _collection.InsertOneAsync(chatMessage, cancellationToken: cancellationToken);

        return Result.Created(new ChatMessageResponse.Create
        {
            Id = chatMessage.Id
        });
    }
    public async Task<Result<List<string>>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken)
    {
        var currentUserId = _sessionContextProvider.User?.GetUserId();
    
        // Haal alle unieke user IDs op waarmee de current user heeft gechat
        var userIds = await _collection
            .Find(m => m.SenderId == currentUserId || m.ReceiverId == currentUserId)
            .Project(m => new { m.SenderId, m.ReceiverId })
            .ToListAsync(cancellationToken);
    
        var uniqueUserIds = userIds
            .SelectMany(x => new[] { x.SenderId, x.ReceiverId })
            .Where(id => id != currentUserId)
            .Distinct()
            .ToList();
    
        // Filter op search term (dit zou je kunnen uitbreiden met Account service)
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            uniqueUserIds = uniqueUserIds
                .Where(id => id.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }
    
        return Result.Success(uniqueUserIds);
    }
}
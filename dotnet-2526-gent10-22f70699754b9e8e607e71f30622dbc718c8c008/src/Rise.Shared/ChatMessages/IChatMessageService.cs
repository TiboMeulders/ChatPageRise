namespace Rise.Shared.ChatMessages;

public interface IChatMessageService
{
    Task<Result<List<ChatMessageResponse.Index>>> GetAllBySenderIdAndReceiverIdAsync(ChatMessageRequest.Index chatMessage, CancellationToken cancellationToken);
    Task<Result<List<ChatMessageResponse.Index>>> GetAllByUserIdAsync(string id, CancellationToken cancellationToken);
    Task<Result<ChatMessageResponse.Create>> CreateAsync(ChatMessageRequest.Create request,
        CancellationToken cancellationToken = default);
}
namespace Rise.Shared.ChatMessages;

public interface IChatMessageService
{
    Task<Result<List<ChatMessageResponse.Index>>> GetAllByLoggedInUserAndReceiverIdAsync(ChatMessageRequest.Index chatMessage, CancellationToken cancellationToken);
    Task<Result<List<ChatMessageResponse.Index>>> GetAllByLoggedInUserAsync(CancellationToken cancellationToken);
    Task<Result<ChatMessageResponse.Create>> CreateAsync(ChatMessageRequest.Create request,
        CancellationToken cancellationToken = default);
    
    Task<Result<List<string>>> SearchUsersAsync(string searchTerm, CancellationToken cancellationToken);
}
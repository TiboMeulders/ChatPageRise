using Rise.Shared.ChatMessages;

namespace Rise.Server.Endpoints.ChatMessages;

public class Index(IChatMessageService chatMessageService): EndpointWithoutRequest<Result<List<ChatMessageResponse.Index>>>
{
    public override void Configure()
    {
        Get("/api/chatmessages");
        AllowAnonymous();
    }

    public override Task<Result<List<ChatMessageResponse.Index>>> ExecuteAsync(CancellationToken ctx)
    {
        var receiverId = Query<string>("receiverId");
        
        var request = new ChatMessageRequest.Index 
        { 
            ReceiverId = receiverId
        };
        
        return chatMessageService.GetAllByLoggedInUserAndReceiverIdAsync(request, ctx);
    }
}


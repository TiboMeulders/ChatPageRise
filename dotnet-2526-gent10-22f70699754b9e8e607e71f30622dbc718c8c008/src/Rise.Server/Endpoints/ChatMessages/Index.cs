using Rise.Shared.ChatMessages;

namespace Rise.Server.Endpoints.ChatMessages;

public class Index(IChatMessageService chatMessageService): Endpoint<ChatMessageRequest.Index, Result<List<ChatMessageResponse.Index>>>
{
    public override void Configure()
    {
        Get("/api/chatmessages");
        AllowAnonymous();
    }

    public override Task<Result<List<ChatMessageResponse.Index>>> ExecuteAsync(ChatMessageRequest.Index req, CancellationToken ctx)
    {
        return chatMessageService.GetAllBySenderIdAndReceiverIdAsync(req, ctx);
    }
}
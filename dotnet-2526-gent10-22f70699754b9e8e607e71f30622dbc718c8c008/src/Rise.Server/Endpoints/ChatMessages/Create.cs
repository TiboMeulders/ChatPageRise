using Rise.Shared.ChatMessages;

namespace Rise.Server.Endpoints.ChatMessages;

public class Create(IChatMessageService chatMessageService): Endpoint<ChatMessageRequest.Create, Result<ChatMessageResponse.Create>>
{
    public override void Configure()
    {
        Post("/api/chatmessages");
        AllowAnonymous();
    }
    
    public override Task<Result<ChatMessageResponse.Create>> ExecuteAsync(ChatMessageRequest.Create req, CancellationToken ctx)
    {
        return chatMessageService.CreateAsync(req, ctx);
    }
}
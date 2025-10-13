using System.Net.Http.Json;
using Rise.Shared.ChatMessages;

namespace Rise.Client.ChatMessages;

public class ChatMessageService(HttpClient httpClient): IChatMessageService
{
    public async Task<Result<List<ChatMessageResponse.Index>>> GetAllBySenderIdAndReceiverIdAsync(ChatMessageRequest.Index chatMessage, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<List<ChatMessageResponse.Index>>>("/api/chatmessages", cancellationToken: ctx);
        return result!;
    }

    public async Task<Result<List<ChatMessageResponse.Index>>> GetAllByUserIdAsync(string id, CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<List<ChatMessageResponse.Index>>>("/api/chatmessages/" + id, ctx);
        return result!;
    }

    public async Task<Result<ChatMessageResponse.Create>> CreateAsync(ChatMessageRequest.Create request, CancellationToken ctx = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/chatmessages", request, ctx);
        var result = await response.Content.ReadFromJsonAsync<Result<ChatMessageResponse.Create>>(cancellationToken: ctx);
        return result!;
    }
}
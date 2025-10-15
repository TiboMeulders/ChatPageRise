using System.Net.Http.Json;
using Rise.Shared.ChatMessages;

namespace Rise.Client.ChatMessages;

public class ChatMessageService(HttpClient httpClient): IChatMessageService
{
    public async Task<Result<List<ChatMessageResponse.Index>>> GetAllByLoggedInUserAndReceiverIdAsync(ChatMessageRequest.Index chatMessage, CancellationToken ctx)
    {
        var url = $"/api/chatmessages?receiverId={chatMessage.ReceiverId}";
        var result = await httpClient.GetFromJsonAsync<Result<List<ChatMessageResponse.Index>>>(url, cancellationToken: ctx);
        return result!;
    }

    public async Task<Result<List<ChatMessageResponse.Index>>> GetAllByLoggedInUserAsync(CancellationToken ctx)
    {
        var result = await httpClient.GetFromJsonAsync<Result<List<ChatMessageResponse.Index>>>("/api/chatmessages/me", ctx);
        return result!;
    }

    public async Task<Result<ChatMessageResponse.Create>> CreateAsync(ChatMessageRequest.Create request, CancellationToken ctx = default)
    {
        var response = await httpClient.PostAsJsonAsync("/api/chatmessages", request, ctx);
        var result = await response.Content.ReadFromJsonAsync<Result<ChatMessageResponse.Create>>(cancellationToken: ctx);
        return result!;
    }
    
    public async Task<Result<List<string>>> SearchUsersAsync(string searchTerm, CancellationToken ctx)
    {
        var url = $"/api/chatmessages/search?searchTerm={Uri.EscapeDataString(searchTerm)}";
        var result = await httpClient.GetFromJsonAsync<Result<List<string>>>(url, cancellationToken: ctx);
        return result!;
    }
}
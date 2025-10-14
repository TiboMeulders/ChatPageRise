using System.Net.Http.Json;
using Rise.Shared.ChatMessages;

namespace Rise.Client.Services.ChatMessages;

using System.Net.Http.Json;
using Rise.Shared.ChatMessages;



public class ChatContactService : IChatContactService
{
    private readonly HttpClient _httpClient;

    public ChatContactService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Result<List<ChatContactDto>>> GetAllContactsAsync(CancellationToken ct = default)
    {
        var result = await _httpClient.GetFromJsonAsync<Result<List<ChatContactDto>>>(
            "/api/chats", ct);
        return result!;
    }
}
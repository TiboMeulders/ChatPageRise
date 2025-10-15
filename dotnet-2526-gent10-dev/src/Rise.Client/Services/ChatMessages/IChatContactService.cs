using Rise.Shared.ChatMessages;

namespace Rise.Client.Services.ChatMessages;

public interface IChatContactService
{
    Task<Result<List<ChatContactDto>>> GetAllContactsAsync(CancellationToken ct = default);
}
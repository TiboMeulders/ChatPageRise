using Microsoft.AspNetCore.Components;
using Rise.Shared.ChatMessages;

namespace Rise.Client.Components;

public partial class ChatWindow
{
    [Inject] public required IChatMessageService ChatMessageService { get; set; }
    private ChatMessageRequest.Create Model { get; set; } = new();

    private async Task CreateChatMessageAsync()
    {
        Model.SentAt = DateTime.Now;
        Model.ReceiverId = "2";
        Model.SenderId = "1";
       await ChatMessageService.CreateAsync(Model);
    }
    
}
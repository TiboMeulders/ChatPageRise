using Microsoft.AspNetCore.SignalR;

namespace Rise.Server.Hubs;

public class GroupChatHub: Hub
{
    public async Task SendMessage(string user, string message)
    {
        await Clients.Users("").SendAsync("ReceiveMessage", user, message);
    }
}
using Microsoft.AspNetCore.SignalR;

namespace Rise.Server.Hubs;

public class PrivateChatHub : Hub
{
    public async Task SendMessage(string user, string message, string receiverId)
    {
        await Clients.User(receiverId).SendAsync("ReceiveMessage", user, message);
    }
}
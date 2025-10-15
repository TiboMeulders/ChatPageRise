using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Rise.Client.Models;
using Rise.Shared.Identity;

namespace Rise.Server.Hubs;

public class PrivateChatHub : Hub
{
    public async Task SendMessage(string receiverId, string payload)
    {
        var user = Context.User;
        
        if (user?.Identity?.IsAuthenticated != true)
        {
            throw new HubException("User not authenticated");
        }
        
        var senderId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        
        if (string.IsNullOrEmpty(senderId))
        {
            throw new HubException("User ID not found in claims");
        }
        
        var message = new ChatMessage
        {
            Payload = payload,
            SenderId = senderId,
            ReceiverId = receiverId,
            Timestamp = DateTime.Now
        };
        
        await Clients.User(receiverId).SendAsync("ReceiveMessage", user.GetUserId(), message);
    }
    private static readonly HashSet<string> _connectedUsers = new();

    public override async Task OnConnectedAsync()
    {
        var userId = Context.User.GetUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            _connectedUsers.Add(userId);
            Console.WriteLine($"🟢 User connected: {userId}");
            
            await Clients.All.SendAsync("UserCameOnline", userId);
        }
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User.GetUserId();
        if (!string.IsNullOrEmpty(userId))
        {
            _connectedUsers.Remove(userId);
            Console.WriteLine($"🔴 User disconnected: {userId}");
            
            await Clients.All.SendAsync("UserWentOffline", userId);
        }
        await base.OnDisconnectedAsync(exception);
    }

    public List<string> GetOnlineUsers()
    {
        var onlineUsers = _connectedUsers.ToList();
        return onlineUsers;
    }
}
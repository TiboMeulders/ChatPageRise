using Microsoft.AspNetCore.SignalR.Client;

namespace Rise.Shared.ChatMessages;

public interface IChatConnectionService
{
    HubConnection Connection { get; }
    Task StartAsync();
    Task StopAsync();
    bool IsConnected { get; }
}
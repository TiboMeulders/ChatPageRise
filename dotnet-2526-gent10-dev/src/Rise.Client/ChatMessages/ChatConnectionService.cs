using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Rise.Shared.ChatMessages;

namespace Rise.Client.ChatMessages;

public class ChatConnectionService : IChatConnectionService
{
    private readonly NavigationManager _navigationManager;
    
    public HubConnection Connection { get; private set; } = default!;
    public bool IsConnected => Connection?.State == HubConnectionState.Connected;

    public ChatConnectionService(NavigationManager navigationManager)
    {
        _navigationManager = navigationManager;
        InitializeConnection();
    }

    private void InitializeConnection()
    {
        Connection = new HubConnectionBuilder()
            .WithUrl(_navigationManager.ToAbsoluteUri("/chathub"))
            .WithAutomaticReconnect()
            .Build();
    }

    public async Task StartAsync()
    {
        if (Connection.State == HubConnectionState.Disconnected)
        {
            await Connection.StartAsync();
        }
    }

    public async Task StopAsync()
    {
        if (Connection.State != HubConnectionState.Disconnected)
        {
            await Connection.StopAsync();
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (Connection is not null)
        {
            await Connection.DisposeAsync();
        }
    }
}
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.SignalR.Client;
using Rise.Client.Identity.Models;
using Rise.Client.Models;
using Rise.Shared.ChatMessages;
using Rise.Shared.Identity;

namespace Rise.Client.Components;

public partial class ChatWindow
{
    [Inject] public required IChatConnectionService ChatConnectionService { get; set; }
    [Inject] public required IChatMessageService ChatMessageService { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    
    [Inject] public required AuthenticationStateProvider SessionContext { get; set; }
    
    [Parameter]
    public Contact Contact { get; set; } = default!;

    [Parameter]
    public EventCallback OnBack { get; set; }

    private ChatMessageRequest.Create Model { get; set; } = new();
    private string _currentMessage = string.Empty;
    private List<ChatMessage> _messages = new();
    private bool _isLoading = true;
    private string _currentUserId = string.Empty;
    private IDisposable? _messageSubscription;

    protected override async Task OnInitializedAsync()
    {
        var authState = await SessionContext.GetAuthenticationStateAsync();
        _currentUserId = authState.User.GetUserId();
        
        _messageSubscription = ChatConnectionService.Connection.On<string, ChatMessage>("ReceiveMessage", (user, message) =>
        {
            _messages.Add(message);
            InvokeAsync(StateHasChanged);
        });

        await LoadMessages();
    }
    protected override void OnParametersSet()
    {
        LoadMessages();
    }

    private async Task LoadMessages()
    {
        _isLoading = true;
        StateHasChanged();

        try
        {
            var request = new ChatMessageRequest.Index 
            { 
                ReceiverId = Contact.Id
            };
            
            var result = await ChatMessageService.GetAllByLoggedInUserAndReceiverIdAsync(request, CancellationToken.None);
            
            if (result.IsSuccess && result.Value != null)
            {
                _messages = result.Value.Select(m => new ChatMessage
                {
                    Payload = m.Payload,
                    IsFromCurrentUser = m.SenderId == _currentUserId,
                    Timestamp = m.SentAt,
                    IsCensored = ContainsBadWords(m.Payload)
                }).ToList();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading messages: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    private async Task SendMessage()
    {
        await CreateChatMessageAsync();
        if (!string.IsNullOrWhiteSpace(Model.Payload))
        {
            var message = new ChatMessage
            {
                Payload = Model.Payload,
                IsFromCurrentUser = true,
                Timestamp = DateTime.Now,
                IsCensored = ContainsBadWords(Model.Payload)
            };

            _messages.Add(message);
            Model.Payload = "";
            StateHasChanged();
        }
    }


    private void HandleKeyDown(KeyboardEventArgs e)
    {
        if (e.Key == "Enter")
        {
            SendMessage();
        }
    }

    private bool ContainsBadWords(string text)
    {
        var badWords = new[] { "stom", "dom", "idioot", "kut", "lul" };
        return badWords.Any(word => text.ToLower().Contains(word));
    }

    private async Task HandleBack() => await OnBack.InvokeAsync();
    private void ShowHelp() { /* TODO: Help tonen */ }
    

    private async Task SendQuickMessage(string text)
    {
        Model.Payload = text;
        await SendMessage();
    }
    private async Task Send()
    {
        if (ChatConnectionService.Connection is not null)
        {
            await ChatConnectionService.Connection.SendAsync("SendMessage", Model.ReceiverId, Model.Payload);
        }
    }
    private async Task CreateChatMessageAsync()
    {
        Model.SentAt = DateTime.Now;
        Model.ReceiverId = Contact.Id;
        await ChatMessageService.CreateAsync(Model);
        await Send();
    }
}
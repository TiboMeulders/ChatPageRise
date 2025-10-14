using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR.Client;
using Rise.Client.Identity.Models;
using Rise.Client.Services;
using Rise.Client.Services.ChatMessages;
using Rise.Shared.ChatMessages;

namespace Rise.Client.Components;

public partial class ChatList : IDisposable
{
    [Inject] public required IChatConnectionService ChatConnectionService { get; set; }
    [Inject] public required IChatContactService ChatContactService { get; set; }
    
    [Parameter]
    public EventCallback<Contact> OnContactSelected { get; set; }

    private List<Contact> contacts = new();
    private HashSet<string> _onlineUsers = new();
    private IDisposable? _onlineSubscription;
    private IDisposable? _offlineSubscription;
    private bool _isLoading = true;
    
    private string searchTerm = string.Empty;
    private List<Contact> filteredContacts = new();

    protected override void OnInitialized()
    {
        // Doe hier niets met filteredContacts - contacts is nog leeg!
        base.OnInitialized();
    }
    
    protected override async Task OnInitializedAsync()
    {
        await LoadContacts();
        
        // ✅ Initialiseer filteredContacts NA het laden van contacts
        filteredContacts = new List<Contact>(contacts);
        StateHasChanged(); // Update UI
        
        await Task.Delay(1000);
        await LoadOnlineStatus();
    
        _onlineSubscription = ChatConnectionService.Connection.On<string>("UserCameOnline", async (userId) =>
        {
            Console.WriteLine($"🟢 Real-time: User came online: {userId}");
            _onlineUsers.Add(userId);
            await InvokeAsync(StateHasChanged);
        });

        _offlineSubscription = ChatConnectionService.Connection.On<string>("UserWentOffline", async (userId) =>
        {
            Console.WriteLine($"🔴 Real-time: User went offline: {userId}");
            _onlineUsers.Remove(userId);
            await InvokeAsync(StateHasChanged);
        });
    }

    private async Task LoadContacts()
    {
        _isLoading = true;
        try
        {
            var result = await ChatContactService.GetAllContactsAsync();
            
            if (result.IsSuccess && result.Value != null)
            {
                contacts = result.Value.Select(c => new Contact
                {
                    Id = c.Id,
                    Name = c.FullName,
                    IsOnline = false,
                    HasSosAlert = false,
                    LastMessage = "",
                    LastMessageTime = null
                }).ToList();
                
                Console.WriteLine($"✅ Loaded {contacts.Count} contacts");
            }
            else
            {
                Console.WriteLine("Failed to load contacts");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error loading contacts: {ex.Message}");
        }
        finally
        {
            _isLoading = false;
            StateHasChanged();
        }
    }

    private async Task LoadOnlineStatus()
    {
        if (ChatConnectionService.IsConnected)
        {
            try
            {
                var onlineUsers = await ChatConnectionService.Connection.InvokeAsync<List<string>>("GetOnlineUsers");
                _onlineUsers = new HashSet<string>(onlineUsers);
                Console.WriteLine($"📊 Loaded {_onlineUsers.Count} online users");
                await InvokeAsync(StateHasChanged);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to get online users: {ex.Message}");
            }
        }
    }

    private bool IsContactOnline(Contact contact)
    {
        return _onlineUsers.Contains(contact.Id);
    }

    private async Task SelectContact(Contact contact)
    {
        await OnContactSelected.InvokeAsync(contact);
    }

    private void ShowHelp()
    {
        // TODO: Toon help overlay
    }
    
    private void OnSearchInput(ChangeEventArgs e)
    {
        searchTerm = e.Value?.ToString() ?? string.Empty;
        Console.WriteLine($"🔍 Zoeken naar: '{searchTerm}'");
        SearchContacts();
    }

    private void SearchContacts()
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            filteredContacts = new List<Contact>(contacts);
            Console.WriteLine($"✅ Toon alle {filteredContacts.Count} contacten");
        }
        else
        {
            filteredContacts = contacts
                .Where(c => c.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
                .ToList();
            Console.WriteLine($"✅ Gevonden: {filteredContacts.Count} contacten");
        }
        StateHasChanged();
    }
    
    public void Dispose()
    {
        _onlineSubscription?.Dispose();
        _offlineSubscription?.Dispose();
    }
}
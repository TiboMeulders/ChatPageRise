using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Threading.Tasks;

namespace Rise.Client.Components
{
    public partial class FriendRequestPopup
    {
        [Parameter] public bool IsVisible { get; set; }
        [Parameter] public object? SelectedPerson { get; set; }
        [Parameter] public EventCallback OnClose { get; set; }
        [Parameter] public EventCallback<object> OnAccept { get; set; }
        [Parameter] public EventCallback<object> OnReject { get; set; }
        
        [Parameter] public string Title { get; set; } = "Vriendschapsverzoek";
        [Parameter] public string Message { get; set; } = "Wil je vrienden worden?";
        [Parameter] public string AcceptButtonText { get; set; } = "Accepteren";
        [Parameter] public string RejectButtonText { get; set; } = "Weigeren";
        [Parameter] public string AcceptButtonClass { get; set; } = "bg-green-500";
        [Parameter] public string AcceptButtonHoverClass { get; set; } = "hover:bg-green-600";
        [Parameter] public string AcceptButtonRingClass { get; set; } = "focus:ring-green-400";

        private string GetPersonName()
        {
            if (SelectedPerson == null) return "";
            
            var nameProperty = SelectedPerson.GetType().GetProperty("Name");
            return nameProperty?.GetValue(SelectedPerson)?.ToString() ?? "";
        }
        
        private string GetFirstName()
        {
            var name = GetPersonName();
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;
            var parts = name.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0] : string.Empty;
        }

        private string GetLastName()
        {
            var name = GetPersonName();
            if (string.IsNullOrWhiteSpace(name)) return string.Empty;
            var parts = name.Trim().Split(' ', System.StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 ? parts[^1] : string.Empty;
        }
        
        private string GetPersonAge()
        {
            if (SelectedPerson == null) return "";
            
            var ageProperty = SelectedPerson.GetType().GetProperty("Age");
            return ageProperty?.GetValue(SelectedPerson)?.ToString() ?? "";
        }

        private async Task ClosePanel()
        {
            await OnClose.InvokeAsync();
        }

        private async Task AcceptRequest()
        {
            if (SelectedPerson != null)
            {
                await OnAccept.InvokeAsync(SelectedPerson);
            }
            await ClosePanel();
        }

        private async Task RejectRequest()
        {
            if (SelectedPerson != null)
            {
                await OnReject.InvokeAsync(SelectedPerson);
            }
            await ClosePanel();
        }
    }
}

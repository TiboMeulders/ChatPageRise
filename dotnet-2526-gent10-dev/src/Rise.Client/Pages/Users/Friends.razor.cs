using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Rise.Client.Services;

namespace Rise.Client.Pages.Users
{
    public partial class Friends
    {
        private List<Friend> _friends = new();

        protected override void OnInitialized()
        {
            PageTitleService.PageTitle = "Vrienden";
            LoadFriends();
        }

        private void LoadFriends()
        {
            _friends = MockData.Friends.ToList();
        }

        private void NavigateToProfile(int userId)
        {
            Navigation.NavigateTo($"/profile/{userId}");
        }

        private static string GetFirstName(string? fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;
            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 0 ? parts[0] : string.Empty;
        }

        private static string GetLastName(string? fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName)) return string.Empty;
            var parts = fullName.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 ? parts[^1] : string.Empty;
        }
    }
}

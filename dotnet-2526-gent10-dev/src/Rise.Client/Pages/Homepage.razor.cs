using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Rise.Client.Services;

namespace Rise.Client.Pages
{
    public partial class Homepage
    {
        protected override void OnInitialized()
        {
            PageTitleService.PageTitle = "Home";
            LoadMockData();
        }

        private bool _showPopup = false;
        private Person? _selectedPerson = null;

        private bool _showSuggestedPopup = false;
        private Person? _selectedSuggestedPerson = null;

        // Mock data using the new MockData service
        private List<Person> _friendshipRequests = new();
        private List<Friend> _friends = new();
        private List<Person> _suggestedFriends = new();

        private void LoadMockData()
        {
            _friendshipRequests = MockData.FriendshipRequests.ToList();
            _friends = MockData.Friends.ToList();
            _suggestedFriends = MockData.SuggestedFriends.ToList();
        }

        private void NavigateToProfile(int userId)
        {
            Navigation.NavigateTo($"/profile/{userId}");
        }

        private void ShowFriendRequestPopup(Person person)
        {
            _selectedPerson = person;
            _showPopup = true;
        }

        private void ClosePopup()
        {
            _showPopup = false;
            _selectedPerson = null;
        }

        private void AcceptFriendRequest(object personObj)
        {
            if (personObj is Person person)
            {
                // Verwijder uit vriendschapsverzoeken en voeg toe aan vrienden
                _friendshipRequests.Remove(person);
                _friends.Add(new Friend { Id = person.Id, Name = person.Name, Age = person.Age, IsOnline = true, Bio = person.Bio, BirthDate = person.BirthDate });

                System.Console.WriteLine($"Vriendschapsverzoek geaccepteerd van {person.Name}");
            }
        }

        private void RejectFriendRequest(object personObj)
        {
            if (personObj is Person person)
            {
                // Verwijder uit vriendschapsverzoeken
                _friendshipRequests.Remove(person);

                System.Console.WriteLine($"Vriendschapsverzoek geweigerd van {person.Name}");
            }
        }

        // Methodes voor voorgestelde vrienden popup
        private void ShowSuggestedFriendPopup(Person person)
        {
            _selectedSuggestedPerson = person;
            _showSuggestedPopup = true;
        }

        private void CloseSuggestedPopup()
        {
            _showSuggestedPopup = false;
            _selectedSuggestedPerson = null;
        }

        private void AddSuggestedFriend(object personObj)
        {
            if (personObj is Person person)
            {
                // Verwijder uit suggesties en voeg toe aan vriendschapsverzoeken
                _suggestedFriends.Remove(person);
                _friendshipRequests.Add(person);

                System.Console.WriteLine($"Vriendschapsverzoek verstuurd naar {person.Name}");
            }
        }

        private void RejectSuggestedFriend(object personObj)
        {
            if (personObj is Person person)
            {
                // Verwijder alleen uit suggesties
                _suggestedFriends.Remove(person);

                System.Console.WriteLine($"Suggestie afgewezen voor {person.Name}");
            }
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

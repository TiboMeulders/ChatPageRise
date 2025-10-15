using System.Collections.Generic;
using Microsoft.AspNetCore.Components;
using Rise.Client.Services;

namespace Rise.Client.Pages.Users
{
    public partial class FriendRequests
    {
        protected override void OnInitialized()
        {
            PageTitleService.PageTitle = "Vriendschapsverzoeken";
        }

        // Popup state
        private bool showPopup = false;
        private Person? selectedPerson = null;

        // Mock data voor vriendschapsverzoeken
        private List<Person> friendshipRequests = new()
        {
            new Person { Name = "Jan", Age = "21" },
            new Person { Name = "Emma", Age = "19" },
            new Person { Name = "Tom", Age = "23" },
            new Person { Name = "Lisa", Age = "20" },
            new Person { Name = "Sarah", Age = "22" },
            new Person { Name = "Mike", Age = "25" },
            new Person { Name = "Laura", Age = "21" },
            new Person { Name = "Kevin", Age = "24" }
        };

        public class Person
        {
            public string Name { get; set; } = string.Empty;
            public string Age { get; set; } = string.Empty;
        }

        private void ShowFriendRequestPopup(Person person)
        {
            selectedPerson = person;
            showPopup = true;
        }

        private void ClosePopup()
        {
            showPopup = false;
            selectedPerson = null;
        }

        private void AcceptFriendRequest(object personObj)
        {
            if (personObj is Person person)
            {
                friendshipRequests.Remove(person);
                System.Console.WriteLine($"Accepted friend request from {person.Name}");
                // Later: Navigate to profile or show success message
            }
        }

        private void RejectFriendRequest(object personObj)
        {
            if (personObj is Person person)
            {
                friendshipRequests.Remove(person);
                System.Console.WriteLine($"Rejected friend request from {person.Name}");
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

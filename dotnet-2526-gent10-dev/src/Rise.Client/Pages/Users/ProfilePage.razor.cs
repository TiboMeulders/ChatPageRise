using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Rise.Client.Services;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Pages.Users;

public partial class ProfilePage
{
    [Inject] private AccountService AccountService { get; set; } = default!;

    private AccountDto.Index? _displayAccount;
    private readonly List<string> _funFacts = new();
    private bool _isLoading = true;
    private string? _loadError;

    // Available fun facts for adding new ones (placeholder data until backend supports them)
    private readonly List<string> _availableFunFacts =
    [
        "Ik voetbal graag",
        "Ik eet graag",
        "Ik speel piano",
        "Ik hou van reizen",
        "Ik ben een nachtmens",
        "Ik hou van koken",
        "Ik lees graag",
        "Ik ben sportief",
        "Ik hou van films",
        "Ik ga graag uit"
    ];

    protected override async Task OnInitializedAsync()
    {
        PageTitleService.PageTitle = "Profiel";
        await LoadUserProfileAsync();
    }

    private async Task LoadUserProfileAsync()
    {
        _isLoading = true;
        _loadError = null;

        try
        {
            var result = await AccountService.GetCurrentAsync();
            if (!result.IsSuccess)
            {
                _loadError = result.Errors.FirstOrDefault() ?? "Kon profiel niet laden.";
                return;
            }

            _displayAccount = result.Value;
            _funFacts.Clear();
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch
        {
            _loadError = "Er ging iets mis bij het laden van het profiel.";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private void AddFunFact()
    {
        var availableFacts = _availableFunFacts.Except(_funFacts).ToList();
        if (availableFacts.Any() && _funFacts.Count < 6)
        {
            _funFacts.Add(availableFacts.First());
            StateHasChanged();
        }
    }

    private void NavigateToEditProfile()
    {
        if (_displayAccount is not null)
        {
            Navigation.NavigateTo($"/edit-profile/{_displayAccount.Id}");
        }
    }

    private static string GetInitials(string? firstName, string? lastName)
    {
        var f = (firstName ?? string.Empty).Trim();
        var l = (lastName ?? string.Empty).Trim();

        char c1 = f.Length > 0 ? char.ToUpperInvariant(f[0]) : '\0';
        char c2 = l.Length > 0 ? char.ToUpperInvariant(l[0]) : '\0';

        if (c1 == '\0' && c2 == '\0') return "?";
        if (c2 == '\0') return c1.ToString();
        if (c1 == '\0') return c2.ToString();
        return new string(new[] { c1, c2 });
    }
}

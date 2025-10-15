using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Rise.Client.Services;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Pages.Users;

public partial class EditProfile
{
    [Parameter] public int UserId { get; set; }

    [Inject] private AccountService AccountService { get; set; } = default!;

    private AccountDto.Index? _account;
    private AccountResponse.Info? _accountInfo;
    private EditUserModel _editedUser = new();
    private bool _isLoading = true;
    private bool _isSaving;
    private string? _loadError;
    private string? _saveError;

    protected override async Task OnInitializedAsync()
    {
        PageTitleService.PageTitle = "Profiel Aanpassen";
        await LoadUserDataAsync();
    }

    private async Task LoadUserDataAsync()
    {
        _isLoading = true;
        _loadError = null;

        try
        {
            var infoResult = await AccountService.GetCurrentInfoAsync();
            if (!infoResult.IsSuccess || infoResult.Value is null)
            {
                _loadError = infoResult.Errors.FirstOrDefault() ?? "Kon accountinformatie niet ophalen.";
                return;
            }

            _accountInfo = infoResult.Value;
            if (_accountInfo.AccountId != UserId)
            {
                Navigation.NavigateTo("/profile");
                return;
            }

            var accountResult = await AccountService.GetByIdAsync(UserId);
            if (!accountResult.IsSuccess || accountResult.Value is null)
            {
                _loadError = accountResult.Errors.FirstOrDefault() ?? "Kon profielgegevens niet ophalen.";
                return;
            }

            _account = accountResult.Value;
            _editedUser = new EditUserModel
            {
                FirstName = _account.FirstName,
                LastName = _account.LastName,
                Pronouns = _account.Pronouns ?? string.Empty,
                Bio = _account.Bio ?? string.Empty,
                Email = _accountInfo.Email,
                BirthDate = _account.BirthDate.ToString("dd/MM/yyyy")
            };
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            _loadError = "Er ging iets mis bij het ophalen van de profielgegevens.";
        }
        finally
        {
            _isLoading = false;
        }
    }

    private async Task SaveProfile()
    {
        if (_account is null)
        {
            return;
        }

        _isSaving = true;
        _saveError = null;

        try
        {
            var request = new AccountRequest.Edit
            {
                Id = _account.Id
            };

            var trimmedFirstName = _editedUser.FirstName?.Trim();
            if (!string.Equals(trimmedFirstName, _account.FirstName, StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(trimmedFirstName))
                {
                    _saveError = "Voornaam mag niet leeg zijn.";
                    return;
                }

                request.FirstName = trimmedFirstName;
            }

            var trimmedLastName = _editedUser.LastName?.Trim();
            if (!string.Equals(trimmedLastName, _account.LastName, StringComparison.Ordinal))
            {
                if (string.IsNullOrWhiteSpace(trimmedLastName))
                {
                    _saveError = "Achternaam mag niet leeg zijn.";
                    return;
                }

                request.LastName = trimmedLastName;
            }

            var trimmedPronouns = _editedUser.Pronouns?.Trim();
            var originalPronouns = _account.Pronouns?.Trim();
            if (!string.Equals(trimmedPronouns ?? string.Empty, originalPronouns ?? string.Empty, StringComparison.Ordinal))
            {
                request.Pronouns = trimmedPronouns ?? string.Empty;
            }

            var trimmedBio = _editedUser.Bio?.Trim() ?? string.Empty;
            var originalBio = _account.Bio?.Trim() ?? string.Empty;
            if (!string.Equals(trimmedBio, originalBio, StringComparison.Ordinal))
            {
                request.Bio = trimmedBio;
            }

            if (request.FirstName is null &&
                request.LastName is null &&
                request.Pronouns is null &&
                request.Bio is null)
            {
                Navigation.NavigateTo("/profile");
                return;
            }

            var result = await AccountService.EditAsync(request);
            if (!result.IsSuccess)
            {
                _saveError = result.Errors.FirstOrDefault() ?? "Kon profiel niet bijwerken.";
                return;
            }

            Navigation.NavigateTo("/profile");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            _saveError = "Er ging iets mis bij het opslaan van het profiel.";
        }
        finally
        {
            _isSaving = false;
        }
    }

    private void CancelEdit()
    {
        Navigation.NavigateTo("/profile");
    }

    private void ChangeProfilePhoto()
    {
        // TODO: Implement profile photo upload functionality
        Console.WriteLine("Opening profile photo picker...");
    }

    private sealed class EditUserModel
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Pronouns { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string BirthDate { get; set; } = string.Empty;
    }
}

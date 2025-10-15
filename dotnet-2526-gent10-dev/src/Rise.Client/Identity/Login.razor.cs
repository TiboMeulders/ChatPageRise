using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Identity;

public partial class Login{
    [SupplyParameterFromQuery] private string? ReturnUrl { get; set; }

    private AccountRequest.Login _model = new();
    private Result _result = new();
    private bool _showPassword;
    
    // Public properties for Razor binding
    public AccountRequest.Login Model => _model;
    public bool ShowPassword => _showPassword;
    
    [Inject] public required IAccountManager AccountManager { get; set; }
    [Inject] public required NavigationManager Navigation { get; set; }
    [Inject] public required AuthenticationStateProvider AuthStateProvider { get; set; }
    
    protected override async Task OnInitializedAsync()
    {
        var authState = await AuthStateProvider.GetAuthenticationStateAsync();
        if (authState.User.Identity?.IsAuthenticated == true)
        {
            Navigation.NavigateTo("/homepage", true);
        }
    }
    
    public async Task LoginUser()
    {
        _result = await AccountManager.LoginAsync(_model.Email!, _model.Password!);

        // If login is successful, redirect after ensuring auth state is updated
        if (_result.IsSuccess){
            // Small delay to ensure authentication state propagates
            await Task.Delay(100);
            
            if (!string.IsNullOrEmpty(ReturnUrl)){
                Navigation.NavigateTo(ReturnUrl);
            }
            else{
                Navigation.NavigateTo("/homepage");
            }
        }
    }
    
    private void TogglePasswordVisibility()
    {
        _showPassword = !_showPassword;
    }
    private async Task QuickLoginUser()
    {
        _result = await AccountManager.LoginAsync("user@example.com", "A1b2C3!");

        // If login is successful, redirect after ensuring auth state is updated
        if (_result.IsSuccess){
            // Small delay to ensure authentication state propagates
            await Task.Delay(100);
            
            if (!string.IsNullOrEmpty(ReturnUrl)){
                Navigation.NavigateTo(ReturnUrl);
            }
            else{
                Navigation.NavigateTo("/homepage");
            }
        }
    }
    
    private async Task QuickLoginAdmin()
    {
        _result = await AccountManager.LoginAsync("admin@example.com", "A1b2C3!");

        // If login is successful, redirect after ensuring auth state is updated
        if (_result.IsSuccess){
            // Small delay to ensure authentication state propagates
            await Task.Delay(100);
            
            if (!string.IsNullOrEmpty(ReturnUrl)){
                Navigation.NavigateTo(ReturnUrl);
            }
            else{
                Navigation.NavigateTo("/homepage");
            }
        }
    }
    
    private async Task QuickLoginEducator()
    {
        _result = await AccountManager.LoginAsync("educator@example.com", "A1b2C3!");

        // If login is successful, redirect after ensuring auth state is updated
        if (_result.IsSuccess){
            // Small delay to ensure authentication state propagates
            await Task.Delay(100);
            
            if (!string.IsNullOrEmpty(ReturnUrl)){
                Navigation.NavigateTo(ReturnUrl);
            }
            else{
                Navigation.NavigateTo("/homepage");
            }
        }
    }
}
using System.Security.Claims;
using Ardalis.Result;
using Microsoft.AspNetCore.Components.Authorization;
using NSubstitute;
using Shouldly;
using Rise.Client.Identity;


namespace Rise.Client.Pages
{
    public class LoginShould : TestContext
    {
        private readonly IAccountManager _accountManager;
        private readonly AuthenticationStateProvider _authStateProvider;

        public LoginShould()
        {
            _accountManager = Substitute.For<IAccountManager>();
            _authStateProvider = Substitute.For<AuthenticationStateProvider>();
        }

        [Fact]
        public async Task RedirectsToHome_WhenUserAlreadyAuthenticated()
        {
            // Arrange
            var authState = new AuthenticationState(
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "test") }, "auth"))
            );
            _authStateProvider.GetAuthenticationStateAsync().Returns(Task.FromResult(authState));

            var nav = Services.GetRequiredService<FakeNavigationManager>();
            var login = new Login
            {
                AccountManager = _accountManager,
                Navigation = nav,
                AuthStateProvider = _authStateProvider
            };

            // Act (OnInitializedAsync is protected → via reflection)
            var method = typeof(Login).GetMethod("OnInitializedAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)method!.Invoke(login, null)!;

            // Assert
            nav.Uri.ShouldContain("/home");
        }

        [Fact]
        public async Task CallsLoginAsync_WhenLoginUserIsCalled()
        {
            // Arrange
            var result = Result.Success();
            _accountManager.LoginAsync("test@example.com", "password")
                .Returns(Task.FromResult(result));

            var nav = Services.GetRequiredService<FakeNavigationManager>();
            var login = new Login
            {
                AccountManager = _accountManager,
                Navigation = nav,
                AuthStateProvider = _authStateProvider
            };

            login.GetType().GetProperty("ReturnUrl",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!
                .SetValue(login, "/home");

            login.Model.Email = "test@example.com";
            login.Model.Password = "password";

            // Act
            await login.LoginUser();

            // Assert
            await _accountManager.Received(1).LoginAsync("test@example.com", "password");
            nav.Uri.ShouldContain("/home");
        }

        [Fact]
        public void TogglesPasswordVisibility_WhenCalled()
        {
            // Arrange
            var login = new Login
            {
                AccountManager = _accountManager,
                Navigation = Services.GetRequiredService<FakeNavigationManager>(),
                AuthStateProvider = _authStateProvider
            };

            var initial = login.ShowPassword;

            var method = typeof(Login).GetMethod("TogglePasswordVisibility",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(login, null);

            // Assert
            login.ShowPassword.ShouldBe(!initial);
        }
    }
}

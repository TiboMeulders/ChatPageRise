using Ardalis.Result;
using Microsoft.AspNetCore.Components;
using NSubstitute;
using Shouldly;
using Rise.Client.Identity;
using Bunit;
using Bunit.TestDoubles;
using Rise.Shared.Identity.Accounts;
using Rise.Shared.Facility;
using Rise.Shared.Common;
using Xunit;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace Rise.Client.Tests.Identity
{
    public class RegisterShould : TestContext
    {
        private readonly IAccountManager _accountManager;
        private readonly IFacilityService _facilityService;

        public RegisterShould()
        {
            _accountManager = Substitute.For<IAccountManager>();
            _facilityService = Substitute.For<IFacilityService>();
        }

        [Fact]
        public async Task LoadsFacilities_OnInitializedAsync_WhenServiceSucceeds()
        {
            // Arrange
            var facilities = new List<FacilityDto.Index>
            {
                new() { Id = 1, Name = "Test Facility" }
            };

            var response = new FacilityResponse.Index
            {
                Facilities = facilities,
                TotalCount = 1
            };

            var successResult = Result<FacilityResponse.Index>.Success(response);

            _facilityService
                .GetIndexAsync(Arg.Any<QueryRequest.SkipTake>())
                .Returns(Task.FromResult(successResult));

            var register = new Register
            {
                AccountManager = _accountManager,
                FacilityService = _facilityService
            };

            // Act
            var method = typeof(Register).GetMethod("OnInitializedAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)method!.Invoke(register, null)!;

            // Assert
            var prop = typeof(Register).GetProperty("FacilitiesDto",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            var list = (List<FacilityDto.Index>)prop.GetValue(register)!;

            list.ShouldNotBeEmpty("FacilitiesDto should contain facilities from the service result");
            list.First().Name.ShouldBe("Test Facility");
        }




        [Fact]
        public async Task DoesNotLoadFacilities_WhenServiceFails()
        {
            // Arrange
            _facilityService.GetIndexAsync(Arg.Any<QueryRequest.SkipTake>())
                .Returns(Task.FromResult(Result<FacilityResponse.Index>.Error("Server error")));


            var register = new Register
            {
                AccountManager = _accountManager,
                FacilityService = _facilityService
            };

            // Act
            var method = typeof(Register).GetMethod("OnInitializedAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            await (Task)method!.Invoke(register, null)!;

            // Assert
            var prop = typeof(Register).GetProperty("FacilitiesDto",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            var list = (List<FacilityDto.Index>)prop.GetValue(register)!;

            list.ShouldBeEmpty();
        }

        [Fact]
        public async Task CallsRegisterAsync_WhenRegisterUserAsyncIsCalled()
        {
            // Arrange
            var result = Result.Success();
            _accountManager.RegisterAsync(Arg.Any<AccountRequest.Register>())
                .Returns(Task.FromResult(result));

            var register = new Register
            {
                AccountManager = _accountManager,
                FacilityService = _facilityService
            };

            // model vullen
            var modelProp = typeof(Register).GetProperty("Model",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!;
            var model = (AccountRequest.Register)modelProp.GetValue(register)!;
            model.Email = "user@example.com";
            model.Password = "password123";

            // Act
            await register.RegisterUserAsync();

            // Assert
            await _accountManager.Received(1).RegisterAsync(Arg.Any<AccountRequest.Register>());
        }

        [Fact]
        public void TogglesPasswordVisibility_WhenCalled()
        {
            var register = new Register
            {
                AccountManager = _accountManager,
                FacilityService = _facilityService
            };

            var initial = typeof(Register).GetProperty("ShowPassword",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(register);

            var method = typeof(Register).GetMethod("TogglePasswordVisibility",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(register, null);

            var after = typeof(Register).GetProperty("ShowPassword",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(register);

            after.ShouldBe(!(bool)initial!);
        }

        [Fact]
        public void TogglesConfirmPasswordVisibility_WhenCalled()
        {
            var register = new Register
            {
                AccountManager = _accountManager,
                FacilityService = _facilityService
            };

            var initial = typeof(Register).GetProperty("ShowConfirmPassword",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(register);

            var method = typeof(Register).GetMethod("ToggleConfirmPasswordVisibility",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            method!.Invoke(register, null);

            var after = typeof(Register).GetProperty("ShowConfirmPassword",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)!.GetValue(register);

            after.ShouldBe(!(bool)initial!);
        }
    }
}

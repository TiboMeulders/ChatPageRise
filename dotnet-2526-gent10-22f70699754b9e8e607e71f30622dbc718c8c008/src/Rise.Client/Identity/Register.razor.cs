using System.Collections.Generic;
using System.Threading.Tasks;
using Ardalis.Result;
using Microsoft.AspNetCore.Components;
using Rise.Shared.Common;
using Rise.Shared.Facility;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Identity;

public partial class Register
{
    [Inject] public required IAccountManager AccountManager { get; set; }
    [Inject] public required IFacilityService FacilityService { get; set; }

    private Result? _result;
    private AccountRequest.Register Model { get; set; } = new();
    private bool ShowPassword { get; set; } = false;
    private bool ShowConfirmPassword { get; set; } = false;

    private List<FacilityDto.Index> FacilitiesDto { get; set; } = new();
    protected override async Task OnInitializedAsync()
    {
        var result = await FacilityService.GetIndexAsync(new QueryRequest.SkipTake { Skip = 0, Take = 100 });
        if (result.IsSuccess && result.Value?.Facilities is not null)
        {
            FacilitiesDto = result.Value.Facilities.ToList();
        }
    }

    public async Task RegisterUserAsync()
    {
        _result = await AccountManager.RegisterAsync(Model);
    }
    
    private void TogglePasswordVisibility()
    {
        ShowPassword = !ShowPassword;
    }
    
    private void ToggleConfirmPasswordVisibility()
    {
        ShowConfirmPassword = !ShowConfirmPassword;
    }
}
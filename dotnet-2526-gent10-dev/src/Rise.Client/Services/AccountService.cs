using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;
using Ardalis.Result;
using Rise.Shared.Identity.Accounts;

namespace Rise.Client.Services;


public class AccountService
{
    private readonly IHttpClientFactory httpClientFactory;

    public AccountService(IHttpClientFactory httpClientFactory)
    {
        this.httpClientFactory = httpClientFactory;
    }


    public async Task<Result<AccountDto.Index>> GetByIdAsync(int accountId, CancellationToken ct = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient("SecureApi");
            var apiResult =
                await client.GetFromJsonAsync<Result<AccountResponse.GetById>>($"/api/users/{accountId}", ct);

            if (apiResult is null)
            {
                return Result<AccountDto.Index>.Error("Kon profielinformatie niet ophalen.");
            }

            if (!apiResult.IsSuccess || apiResult.Value?.Account is null)
            {
                return Result<AccountDto.Index>.Error(apiResult.Errors.FirstOrDefault() ?? "Kon profielinformatie niet ophalen.");
            }

            return Result<AccountDto.Index>.Success(apiResult.Value.Account);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            return Result<AccountDto.Index>.Error("Kon profielinformatie niet ophalen.");
        }
    }

    public async Task<Result<AccountResponse.Info>> GetCurrentInfoAsync(CancellationToken ct = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient("SecureApi");
            var infoResult =
                await client.GetFromJsonAsync<Result<AccountResponse.Info>>("/api/identity/accounts/info", ct);

            if (infoResult is null)
            {
                return Result<AccountResponse.Info>.Error("Kon accountinformatie niet ophalen.");
            }

            if (!infoResult.IsSuccess || infoResult.Value is null)
            {
                return Result<AccountResponse.Info>.Error(infoResult.Errors.FirstOrDefault() ?? "Kon accountinformatie niet ophalen.");
            }

            return Result<AccountResponse.Info>.Success(infoResult.Value);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            return Result<AccountResponse.Info>.Error("Kon accountinformatie niet ophalen.");
        }
    }

    public async Task<Result<AccountDto.Index>> GetCurrentAsync(CancellationToken ct = default)
    {
        try
        {
            var infoResult = await GetCurrentInfoAsync(ct);
            if (!infoResult.IsSuccess)
            {
                return Result<AccountDto.Index>.Error(infoResult.Errors.FirstOrDefault() ?? "Kon accountinformatie niet ophalen.");
            }

            if (infoResult.Value.AccountId <= 0)
            {
                return Result<AccountDto.Index>.Error("Account-ID van de gebruiker ontbreekt.");
            }

            return await GetByIdAsync(infoResult.Value.AccountId, ct);
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            return Result<AccountDto.Index>.Error("Kon accountinformatie niet ophalen.");
        }
    }

    public async Task<Result> EditAsync(AccountRequest.Edit request, CancellationToken ct = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient("SecureApi");
            var response = await client.PutAsJsonAsync($"/api/users/{request.Id}", request, ct);

            Result? apiResult = null;
            try
            {
                apiResult = await response.Content.ReadFromJsonAsync<Result>(cancellationToken: ct);
            }
            catch (Exception ex) when (ex is NotSupportedException or System.Text.Json.JsonException)
            {
                // Response body might be empty or not JSON; ignore and fall back to status code.
            }

            if (apiResult is not null)
            {
                return apiResult;
            }

            if (response.IsSuccessStatusCode)
            {
                return Result.Success();
            }

            return Result.Error("Kon profiel niet bijwerken.");
        }
        catch (OperationCanceledException)
        {
            throw;
        }
        catch (Exception)
        {
            return Result.Error("Kon profiel niet bijwerken.");
        }
    }
}

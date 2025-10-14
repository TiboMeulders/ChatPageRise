using Rise.Shared.Common;
using Rise.Shared.Facility;
using Rise.Shared.Identity.Accounts;

namespace Rise.Server.Endpoints.Identity.Accounts;

public class GetById(IAccountService accountService) : Endpoint<AccountRequest.GetById, Result<AccountResponse.GetById>>
{
    public override void Configure()
    {
        Get("/api/users");
    }

    public override Task<Result<AccountResponse.GetById>> ExecuteAsync(AccountRequest.GetById req, CancellationToken ct)
    {
        return accountService.GetByIdAsync(req, ct);
    }
}
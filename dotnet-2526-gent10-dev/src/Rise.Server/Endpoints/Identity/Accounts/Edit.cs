using Rise.Shared.Identity.Accounts;

namespace Rise.Server.Endpoints.Identity.Accounts;

public class Edit(IAccountService accountService)  : Endpoint<AccountRequest.Edit, Result>
{
    public override void Configure()
    {
        Put("/api/users/{id}");
    }

    public override Task<Result> ExecuteAsync(AccountRequest.Edit req, CancellationToken ctx)
    {
        return accountService.EditAsync(req, ctx);
    }
}
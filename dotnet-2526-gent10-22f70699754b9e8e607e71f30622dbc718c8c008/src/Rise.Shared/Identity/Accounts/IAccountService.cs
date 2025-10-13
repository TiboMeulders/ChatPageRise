using Rise.Shared.Common;

namespace Rise.Shared.Identity.Accounts;

public interface IAccountService
{
     Task<Result<AccountResponse.GetById>> GetByIdAsync(AccountRequest.GetById request, CancellationToken ctx = default);
}
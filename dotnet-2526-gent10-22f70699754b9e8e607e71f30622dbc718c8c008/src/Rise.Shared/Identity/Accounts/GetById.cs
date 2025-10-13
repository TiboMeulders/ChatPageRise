namespace Rise.Shared.Identity.Accounts;

public static partial class AccountRequest
{
    public class GetById
    {
        public required int AccountId { get; set; }

        public class Validator : AbstractValidator<GetById>
        {
            public Validator()
            {
                RuleFor(x => x.AccountId).NotNull();
            }
        }
    }
}
public static partial class AccountResponse
{
    public class GetById
    {
        public required AccountDto.Index Account { get; set; }
    }
}   
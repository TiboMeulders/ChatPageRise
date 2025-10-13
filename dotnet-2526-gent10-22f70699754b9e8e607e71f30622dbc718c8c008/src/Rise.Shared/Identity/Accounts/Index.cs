namespace Rise.Shared.Identity.Accounts;
public static partial class AccountResponse
{
    public class Index
    {
        public IEnumerable<AccountDto.Index> Accounts { get; set; } = [];
        public int TotalCount { get; set; }
    }
}   
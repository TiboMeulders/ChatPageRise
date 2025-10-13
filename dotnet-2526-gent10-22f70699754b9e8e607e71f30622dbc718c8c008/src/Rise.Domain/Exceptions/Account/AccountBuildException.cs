namespace Rise.Domain.Exceptions.Account;

public class AccountBuildException(List<AccountBuildRules> _rules) : Exception
{
    public List<AccountBuildRules> GetRules()
    { 
        return _rules;
    }
}
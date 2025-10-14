//using Rise.Domain.Exceptions;
//using Rise.Domain.Exceptions.Account;

using Microsoft.AspNetCore.Identity;

namespace Rise.Domain.Account;

public class Account : Entity
{
    public required string UserId {get; set;}
    public required string FirstName{get; set;} //TODO - Init in plaats van set?
    public required string LastName{get; set;}
    public required DateOnly BirthDate{get; set;}
    //private Gender _gender;
    public string? Pronouns { get; set; }
    public string? Bio { get; set; }
    public required Facility.Facility  Facility{ get; set; }

    
    /*
    private string? _name;
    private string? _firstName;
    private string _email;
    private string _passwordHash;
    private DateOnly? _birthDate;
    //private Gender _gender;
    private string? _pronouns;

    public Account(AccountBuilder builder)
    {
        this._name = builder._name;
        this._firstName = builder._firstName;
        this._email = builder._email;
        this._passwordHash = builder._passwordHash;
        this._birthDate = builder._birthDate;
        this._pronouns = builder._pronouns;
    }

    public AccountBuilder GetBuilder()
    {
        return new AccountBuilder();
    }
    */
}

/*
public class AccountBuilder{
    internal string? _name;
    internal string? _firstName;
    internal string? _email;
    internal string? _passwordHash;
    internal DateOnly? _birthDate;
    //internal Gender _gender;
    internal string? _pronouns;

    public AccountBuilder WithName(string name)
    {
        _name = name;
        return this;
    }

    public AccountBuilder WithFirstName(string firstName)
    {
        _firstName = firstName;
        return this;
    }

    public AccountBuilder WithEmail(string email)
    {
        _email = email;
        return this;
    }

    public AccountBuilder WithPasswordHash(string passwordHash)
    {
        _passwordHash = passwordHash;
        return this;
    }

    public AccountBuilder WithBirthDate(DateOnly birthDate)
    {
        _birthDate = birthDate;
        return this;
    }

    public AccountBuilder WithPronouns(string pronouns)
    {
        _pronouns = pronouns;
        return this;
    }

    public Account Build()
    {
        //TODO - Build validaties toevoegen
        List<AccountBuildRules> rules = new List<AccountBuildRules>();
        
        if(this._email is null)
            rules.Add(AccountBuildRules.EMAIL_REQUIRED);
        
        if(this._passwordHash is null)
            rules.Add(AccountBuildRules.PASSWORD_REQUIRED);

        if (rules.Count != 0)
            throw new AccountBuildException(rules);
        
        return new Account(this);
    }

    internal AccountBuilder(){
    }
}
*/
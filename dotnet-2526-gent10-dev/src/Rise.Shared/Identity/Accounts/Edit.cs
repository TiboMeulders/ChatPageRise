namespace Rise.Shared.Identity.Accounts;

public static partial class AccountRequest
{
    public class Edit
    {
        public required int Id { get; set; }
        
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Pronouns { get; set; }
        public string? Bio { get; set; }
        
        public class Validator : AbstractValidator<Edit>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty().When(x => x.FirstName != null);
                RuleFor(x => x.LastName).NotEmpty().When(x => x.LastName != null);
                RuleFor(x => x.Pronouns).NotEmpty().When(x => x.Pronouns != null);
            }
        }
    }
}
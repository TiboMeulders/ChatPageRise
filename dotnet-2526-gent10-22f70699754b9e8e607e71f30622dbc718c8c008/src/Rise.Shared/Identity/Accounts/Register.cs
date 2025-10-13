using Destructurama.Attributed;
using Rise.Shared.Facility;

namespace Rise.Shared.Identity.Accounts;

/// <summary>
/// Represents a request structure for account-related operations, such as registration or authentication.
/// </summary>
public static partial class AccountRequest
{
    public class Register
    {
        /// <summary>
        /// The user's first name.
        /// </summary>
        public string? FirstName { get; set; }
        
        /// <summary>
        /// The user's last name.
        /// </summary>
        public string? LastName { get; set; }
        
        /// <summary>
        /// The user's birth date.
        /// </summary>
        public DateOnly? BirthDate { get; set; }
        
        /// <summary>
        /// The user's email address which acts as a user name.
        /// </summary>
        public string? Email { get; set; }

        /// <summary>
        /// The user's password.
        /// </summary>
        [LogMasked]
        public string? Password { get; set; }
        
        /// <summary>
        /// The user's password.
        /// </summary>
        [LogMasked]
        public string? ConfirmPassword { get; set; }
        
        // Other needed stuff here, like Role(s), Firstname, lastname etc.
        
        public string? Bio { get; set; }
        
        public int? FacilityId { get; set; }
        

        /// <summary>
        /// Provides validation rules for the Register class fields such as email and password.
        /// </summary>
        public class Validator : AbstractValidator<Register>
        {
            public Validator()
            {
                RuleFor(x => x.FirstName).NotEmpty().WithMessage("Voornaam is verplicht.");
                RuleFor(x => x.LastName).NotEmpty().WithMessage("Naam is verplicht.");
                RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Geboortedatum is verplicht.")
                    .LessThan(DateOnly.FromDateTime(DateTime.Now)).WithMessage("Geboortedatum moet in het verleden liggen.");
                RuleFor(x => x.Email).NotEmpty().EmailAddress();
                RuleFor(x => x.Password).NotEmpty();
                RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage("Passwords do not match.");
                RuleFor(x => x.FacilityId).NotEmpty().WithMessage("Facility is verplicht.");
            }
        }
    }
}
namespace Rise.Shared.Identity.Accounts;

public static class AccountDto
{
    public class Index
    {
        public required int Id { get; set; }
        
        public required string UserId { get; set; }
        public required string FirstName { get; set; }
        public required string LastName { get; set; }
        public required DateOnly BirthDate { get; set; }
        public string? Pronouns { get; set; }

        public string? Bio { get; set; }
        public required Facility.FacilityDto.Index FacilityDto { get; set; }
    }
}
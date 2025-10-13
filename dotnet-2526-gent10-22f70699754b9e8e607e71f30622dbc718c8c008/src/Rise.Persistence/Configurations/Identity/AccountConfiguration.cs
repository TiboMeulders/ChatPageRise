using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rise.Domain.Account;

namespace Rise.Persistence.Configurations.Identity;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("Account");
        builder.Property(a => a.UserId).HasMaxLength(255);
        builder.Property(a => a.FirstName).HasMaxLength(32);
        builder.Property(a => a.LastName).HasMaxLength(32);
        builder.Property(a => a.Bio).HasMaxLength(190);
        builder.Property(a => a.Pronouns).HasMaxLength(100);
    }
}
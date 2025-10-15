using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rise.Persistence.Configurations.Facility;

internal class FacilityConfiguration : EntityConfiguration<Domain.Facility.Facility>
{
    public override void Configure(EntityTypeBuilder<Domain.Facility.Facility> builder)
    {
        base.Configure(builder);
        // Other Product configuration here.
        builder.Property(x => x.Name).HasMaxLength(100).IsRequired();
    }
}
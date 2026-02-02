using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    public class ListingConfiguration : IEntityTypeConfiguration<Listing>
    {
        public void Configure(EntityTypeBuilder<Listing> builder)
        {
            builder.HasKey(l => l.Id);

            builder.Property(l => l.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(l => l.City)
                .WithMany()
                .HasForeignKey(l => l.CityId);

            builder.HasOne(l => l.Country)
                .WithMany()
                .HasForeignKey(l => l.CountryId);

            builder.HasOne(l => l.SaleDetails)
                .WithOne(sd => sd.Listing)
                .HasForeignKey<SaleDetails>(sd => sd.ListingId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(l => l.BuyDetails)
               .WithOne(bd => bd.Listing)
               .HasForeignKey<BuyDetails>(bd => bd.ListingId)
               .OnDelete(DeleteBehavior.Cascade);
        }
    }
}

using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(c => c.LocationCountry)
                .WithMany()
                .HasForeignKey(c => c.LocationCountryId);

            builder.HasOne(c => c.LocationCity)
                .WithMany()
                .HasForeignKey(c => c.LocationCityId);

            builder.HasMany(c => c.OwnershipHistory)
                .WithOne(oh => oh.Car)
                .HasForeignKey(oh => oh.CarId);

            builder.HasOne(c => c.Brand)
                .WithMany()
                .HasForeignKey(c => c.BrandId);
        }
    }
}
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

            builder.HasOne(c => c.LocationCity)
                .WithMany(ci => ci.Cars)
                .HasForeignKey(c => c.LocationCityId);

            builder.HasMany(c => c.OwnershipHistory)
                .WithOne(oh => oh.Car)
                .HasForeignKey(oh => oh.CarId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(c => c.Model)
                .WithMany(m => m.Cars)
                .HasForeignKey(c => c.ModelId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
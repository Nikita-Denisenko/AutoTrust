using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    internal class SaleDetailsConfiguration : IEntityTypeConfiguration<SaleDetails>
    {
        public void Configure(EntityTypeBuilder<SaleDetails> builder)
        {
            builder.HasKey(sd => sd.Id);

            builder.Property(sd => sd.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(sd => sd.Car)
                .WithOne()
                .HasForeignKey<SaleDetails>(sd => sd.CarId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
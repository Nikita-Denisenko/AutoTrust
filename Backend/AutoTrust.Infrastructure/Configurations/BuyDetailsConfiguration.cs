using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    public class BuyDetailsConfiguration : IEntityTypeConfiguration<BuyDetails>
    {
        public void Configure(EntityTypeBuilder<BuyDetails> builder)
        {
            builder.HasKey(bd => bd.Id);

            builder.Property(bd => bd.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(bd => bd.Brand)
                .WithMany()
                .HasForeignKey(bd => bd.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
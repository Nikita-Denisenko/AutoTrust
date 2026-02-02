using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    public class CarOwnershipConfiguration : IEntityTypeConfiguration<CarOwnership>
    {
        public void Configure(EntityTypeBuilder<CarOwnership> builder)
        {
            builder.HasKey(co => co.Id);

            builder.Property(co => co.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(co => co.User)
                .WithMany()
                .HasForeignKey(co => co.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
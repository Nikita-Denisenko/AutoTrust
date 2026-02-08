using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    public class ReactionConfiguration : IEntityTypeConfiguration<Reaction>
    {
        public void Configure(EntityTypeBuilder<Reaction> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(r => r.User)
                .WithMany()
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(r => r.Listing)
                .WithMany()
                .HasForeignKey(r => r.ListingId)
                .OnDelete(DeleteBehavior.Cascade);
            
            builder.HasOne(m => )
        }
    }
}
using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    public class ModelConfiguration : IEntityTypeConfiguration<Model>
    {
        public void Configure(EntityTypeBuilder<Model> builder)
        {
            builder.HasKey(m => m.Id);

            builder.Property(m => m.Id)
                .ValueGeneratedOnAdd();

            builder.HasOne(m => m.Brand)
                .WithMany()
                .HasForeignKey(m => m.BrandId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
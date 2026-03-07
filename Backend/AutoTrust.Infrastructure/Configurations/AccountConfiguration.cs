using AutoTrust.Domain.Entities;
using AutoTrust.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.Id)
                .ValueGeneratedOnAdd();

            builder.Property(a => a.Email)
                .HasConversion
                (
                    email => email.Value,
                    value => new Email(value)
                );

            builder.Property(a => a.Phone)
                .HasConversion
                (
                    phone => phone.Value,
                    value => new Phone(value)
                );
        }
    }
}

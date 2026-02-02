using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AutoTrust.Infrastructure.Configurations
{
    public class ChatConfiguration : IEntityTypeConfiguration<Chat>
    {
        public void Configure(EntityTypeBuilder<Chat> builder)
        {
            builder.HasKey(ch => ch.Id);

            builder.Property(ch => ch.Id)
                .ValueGeneratedOnAdd();

            builder.HasMany(ch => ch.ChatParticipants)
                .WithOne(cp => cp.Chat)
                .HasForeignKey(cp => cp.ChatId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
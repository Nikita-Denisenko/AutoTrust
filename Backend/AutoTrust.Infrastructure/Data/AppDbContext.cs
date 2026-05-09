using AutoTrust.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AutoTrust.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<CarOwnership> CarsOwnerships { get; set; }
        public DbSet<Chat> Chats { get; set; }
        public DbSet<ChatParticipant> ChatPartsicipants { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Follow> Follows { get; set; }
        public DbSet<Listing> Listings { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Model> Models { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<SaleDetails> SaleDetails { get; set; }
        public DbSet<BuyDetails> BuyDetails { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql("Server=localhost;Port=3306;Database=AutoTrust;User=root;Password=root;",
                    new MySqlServerVersion(new Version(8, 0, 0)));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var dateOnlyConverter = new ValueConverter<DateOnly, DateTime>(
               v => v.ToDateTime(TimeOnly.MinValue),
               v => DateOnly.FromDateTime(v)
           );

            modelBuilder.Entity<User>(entity =>
            {
                entity.OwnsOne(u => u.BirthDate, birthDate =>
                {
                    birthDate.Property(b => b.Value)
                        .HasColumnName("BirthDate")
                        .HasConversion(dateOnlyConverter);
                });
            });

            modelBuilder.Entity<Car>(entity =>
            {
                entity.OwnsOne(c => c.StateNumber, stateNumber =>
                {
                    stateNumber.Property(s => s.Value)
                        .HasColumnName("StateNumber")
                        .HasMaxLength(9) 
                        .IsRequired();
                });

                entity.OwnsOne(c => c.ImageUrl, imageUrl =>
                {
                    imageUrl.Property(u => u.Value)
                        .HasColumnName("ImageUrl")
                        .HasMaxLength(500);
                });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.OwnsOne(u => u.BirthDate, birthDate =>
                {
                    birthDate.Property(b => b.Value)
                        .HasColumnName("BirthDate")
                        .HasConversion(dateOnlyConverter);
                });

                entity.OwnsOne(u => u.AvatarUrl, avatarUrl =>
                {
                    avatarUrl.Property(a => a.Value)
                        .HasColumnName("AvatarUrl")
                        .HasMaxLength(500);
                });
            });

            modelBuilder.Entity<Brand>(entity =>
            {
                entity.OwnsOne(u => u.LogoUrl, birthDate =>
                {
                    birthDate.Property(b => b.Value)
                        .HasColumnName("LogoUrl")
                        .HasMaxLength(500);
                });
            });

            modelBuilder.Entity<CarOwnership>(entity =>
            {
                entity.OwnsOne(u => u.BillOfSalePhotoUrl, birthDate =>
                {
                    birthDate.Property(b => b.Value)
                        .HasColumnName("BillOfSalePhotoUrl")
                        .HasMaxLength(500);
                });
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.OwnsOne(u => u.FlagImageUrl, birthDate =>
                {
                    birthDate.Property(b => b.Value)
                        .HasColumnName("FlagImageUrl")
                        .HasMaxLength(500);
                });
            });

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly
            );
        }
    }
}
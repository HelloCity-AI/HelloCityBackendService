using HelloCity.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace HelloCity.Api.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Users> Users { get; set;}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Users>(entity =>
            {
                entity
                    .HasKey(u => u.UserId);

                entity
                    .Property(u => u.Username).IsRequired().HasMaxLength(50);
                entity.HasIndex(u => u.Username).IsUnique();

                entity
                    .Property(u => u.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                entity
                    .HasIndex(u => u.Email)
                    .IsUnique();

                entity
                    .Property(u => u.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity
                    .Property(u => u.Avatar)
                    .HasMaxLength(255);

                entity
                    .Property(u => u.Gender)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity
                    .Property(u => u.Nationality)
                    .HasMaxLength(100);

                entity
                    .Property(u => u.University)
                    .HasMaxLength(100);

                entity
                    .Property(u => u.Major)
                    .HasMaxLength(100);

                entity.Property(u => u.PreferredLanguage)
                    .HasConversion<string>()
                    .HasMaxLength(20);

                entity
                    .Property(u => u.LastJoinDate)
                    .IsRequired();

                entity
                    .Property(u => u.CreatedAt)
                    .IsRequired();

                entity
                    .Property(u => u.UpdatedAt)
                    .IsRequired();
            });
        }
    }
}

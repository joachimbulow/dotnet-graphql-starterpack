using Microsoft.EntityFrameworkCore;
using Starterpack.Auth.Persistance.Entities;

namespace Starterpack.Common.Api
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<User.Persistance.Entities.UserEntity> Users { get; set; }

        public DbSet<Auth.Persistance.Entities.RefreshTokenEntity> RefreshTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // For UUID generation with Postgres
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<RefreshTokenEntity>()
            .HasIndex(token => token.Token)
            .IncludeProperties(token => token.ExpiresAt);

            // modelBuilder
            //     .Entity<Platform>()
            //     .HasMany(p => p.Commands)
            //     .WithOne(c => c.Platform!)
            //     .HasForeignKey(c => c.PlatformId);
        }
    }
}
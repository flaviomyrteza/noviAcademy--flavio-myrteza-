using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using WorldRank.Domain.Entities;

namespace WorldRanInfastracture.Persistence;

public class WorldRankDbContext : DbContext
{
    public WorldRankDbContext(DbContextOptions<WorldRankDbContext> options)
        : base(options)
    {
    }

    public DbSet<Player> Players { get; set; }
    public DbSet<Wallet> Wallets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Player>(entity =>
        {
            entity.HasKey(p => p.Id);

           entity.Property(p => p.Id)
          .ValueGeneratedNever();

            entity.Property(p => p.Name)
                  .IsRequired()
                  .HasMaxLength(100);

            entity.Property(p => p.Score)
                  .IsRequired();
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(w => w.Id);

            entity.Property(w => w.Id)
            .ValueGeneratedNever();

            entity.Property(w => w.Currency)
                  .HasConversion<string>()
                  .HasMaxLength(10)
                  .IsRequired();

            entity.Property(w => w.Balance)
                  .HasColumnType("decimal(18,2)")
                  .IsRequired();

            entity.Property(w => w.IsBlocked)
                  .IsRequired();

            entity.HasOne<Player>()
                  .WithMany()
                  .HasForeignKey(w => w.PlayerId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
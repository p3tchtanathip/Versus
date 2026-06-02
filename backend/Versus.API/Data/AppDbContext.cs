using Microsoft.EntityFrameworkCore;
using Versus.API.Models;

namespace Versus.API.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Session> Sessions => Set<Session>();
        public DbSet<TierList> TierList => Set<TierList>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Item> Items => Set<Item>();
        public DbSet<Match> Matches => Set<Match>();
        public DbSet<EloHistory> EloHistories => Set<EloHistory>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Category
            modelBuilder.Entity<Category>()
                .HasData(
                    new Category
                    {
                        Id = 1,
                        Name = "Movies",
                        Slug = "movies",
                        IconUrl = "https://fontawesome.com/icons/classic/solid/clapperboard",
                        IsActive = true
                    },
                    new Category
                    {
                        Id = 2,
                        Name = "Series",
                        Slug = "series",
                        IconUrl = "https://fontawesome.com/icons/classic/solid/desktop",
                        IsActive = true
                    },
                    new Category
                    {
                        Id = 3,
                        Name = "Music",
                        Slug = "music",
                        IconUrl = "https://fontawesome.com/icons/classic/solid/music",
                        IsActive = true
                    },
                    new Category
                    {
                        Id = 4,
                        Name = "Sport",
                        Slug = "sport",
                        IconUrl = "https://fontawesome.com/icons/classic/solid/person-running",
                        IsActive = true
                    }
                );

            // Match -> TierList
            modelBuilder.Entity<Match>()
                .HasOne(m => m.TierList)
                .WithMany(t => t.Matches)
                .HasForeignKey(m => m.TierListId)
                .OnDelete(DeleteBehavior.Restrict);

            // Winner
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Winner)
                .WithMany(i => i.WinnerMatches)
                .HasForeignKey(m => m.WinnerId)
                .OnDelete(DeleteBehavior.Restrict);

            // Loser
            modelBuilder.Entity<Match>()
                .HasOne(m => m.Loser)
                .WithMany(i => i.LoserMatches)
                .HasForeignKey(m => m.LoserId)
                .OnDelete(DeleteBehavior.Restrict);

            // EloHistory
            modelBuilder.Entity<EloHistory>()
                .HasOne(e => e.Match)
                .WithMany(m => m.EloHistories)
                .HasForeignKey(e => e.MatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Unique Index
            modelBuilder.Entity<Category>()
                .HasIndex(c => c.Slug)
                .IsUnique();
        }
    }
}


using GPMuseumify.DAL.Models;
using Microsoft.EntityFrameworkCore;


namespace GPMuseumify.DAL.Configuration;

public class ApplicationDbContext: DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Statue> Statues { get; set; }
    public DbSet<Museum> Museums { get; set; }
    public DbSet<StatueImage> StatueImages { get; set; }
    public DbSet<UserHistory> UserHistories { get; set; }
    public DbSet<UserFavorite> UserFavorites { get; set; }
    public DbSet<Story> Stories { get; set; }
    public DbSet<ImageRecognitionLog> ImageRecognitionLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User Configuration
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Email).IsRequired();
            entity.HasIndex(e => e.Role); // Index for Role queries
        });

        // UserFavorite - Prevent duplicate favorites
        modelBuilder.Entity<UserFavorite>(entity =>
        {
            entity.HasIndex(e => new { e.UserId, e.StatueId }).IsUnique()
                .HasFilter("[StatueId] IS NOT NULL");
            entity.HasIndex(e => new { e.UserId, e.MuseumId }).IsUnique()
                .HasFilter("[MuseumId] IS NOT NULL");
        });


        // Statue Configuration
        modelBuilder.Entity<Statue>(entity =>
        {
            entity.HasIndex(e => e.Name);
            entity.Property(e => e.VideoUrl).IsRequired();
        });

        // StatueImage Configuration
        modelBuilder.Entity<StatueImage>(entity =>
        {
            entity.HasOne(si => si.Statue)
                .WithMany(s => s.StatueImages)
                .HasForeignKey(si => si.StatueId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // UserHistory Configuration
        modelBuilder.Entity<UserHistory>(entity =>
        {
            entity.HasOne(uh => uh.User)
                .WithMany(u => u.UserHistories)
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // UserFavorite Configuration
        modelBuilder.Entity<UserFavorite>(entity =>
        {
            entity.HasOne(uf => uf.User)
                .WithMany(u => u.UserFavorites)
                .HasForeignKey(uf => uf.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}


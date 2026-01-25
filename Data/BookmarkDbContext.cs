
using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Data;

public class BookmarkDbContext : DbContext
{
    public BookmarkDbContext(DbContextOptions<BookmarkDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BookmarkTag> BookmarkTags { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("bookmark");
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.UserId);

            entity.HasIndex(u => u.Email).IsUnique();

            entity.Property(u => u.FullName).IsRequired();
            entity.Property(u => u.Email).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(u => u.UpdatedAt).ValueGeneratedOnAddOrUpdate();

            entity.HasMany(u => u.Bookmarks).WithOne(u => u.User).HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.ToTable("bookmarks");
            entity.HasKey(e => e.BookmarkId);

            entity.HasIndex(e => new { e.UserId, e.Title }).IsUnique();
            entity.HasIndex(e => new { e.UserId, e.Url }).IsUnique();

            entity.Property(e => e.Title).IsRequired();
            entity.Property(e => e.Description).IsRequired();
            entity.Property(e => e.Url).IsRequired();
            entity.Property(e => e.IsPinned).HasDefaultValue(false);
            entity.Property(e => e.IsArchived).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UpdateAt).ValueGeneratedOnAddOrUpdate();

            entity.HasMany(e => e.Visits).WithOne(e => e.Bookmark).HasForeignKey(e => e.BookmarkId).OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.BookmarkTags).WithOne(e => e.Bookmark).HasForeignKey(e => e.BookmarkId).OnDelete(DeleteBehavior.Cascade);

        });

        modelBuilder.Entity<Visit>(entity =>
        {
            entity.ToTable("visits");
            entity.HasKey(e => e.VisitId);

            entity.HasIndex(e => e.BookmarkId);
            entity.HasIndex(e => e.VisitDateAt);

            entity.Property(e => e.VisitDateAt).HasDefaultValueSql("NULL");
        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.ToTable("tags");
            entity.HasKey(e => e.TagId);

            entity.HasIndex(e => e.Name).IsUnique();

            entity.Property(e => e.Name).IsRequired().HasMaxLength(50);

            entity.HasMany(e => e.BookmarkTags).WithOne(e => e.Tag).HasForeignKey(e => e.TagId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<BookmarkTag>(entity =>
        {
            entity.ToTable("bookmark_tags");
            entity.HasKey(e => e.BookmarkTagId);

            entity.HasIndex(e => new { e.BookmarkId, e.TagId }).IsUnique();
        });
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is User || e.Entity is Bookmark)
            .Where(e => e.State == EntityState.Modified);

        foreach (var entityEntry in entries)
        {
            if (entityEntry.Entity is User user)
            {
                user.UpdatedAt = DateTime.UtcNow;
            }
            else if (entityEntry.Entity is Bookmark bookmark)
            {
                bookmark.UpdateAt = DateTime.UtcNow;
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        return base.SaveChangesAsync(cancellationToken);
    }
}
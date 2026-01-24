
using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Data;

public class BookmarkDbContext : DbContext
{
    public BookmarkDbContext(DbContextOptions<BookmarkDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("bookmark");
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Username).IsRequired();
            entity.Property(u => u.Email).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
        });
        modelBuilder.Entity<Bookmark>(entity =>
        {
            entity.ToTable("bookmarks");
            entity.HasKey(b => b.Id);
            entity.Property(b => b.Title).IsRequired();
            entity.Property(b => b.Description).IsRequired();
            entity.Property(b => b.Url).IsRequired();
            entity.Property(b => b.Tags).HasColumnType("text[]");
            entity.Property(b => b.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(b => b.VisitedLastAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.HasIndex(b => b.UserId);
            entity.HasIndex(b => b.CreatedAt);
            entity.HasIndex(b => b.Url);
            entity.HasOne<User>().WithMany().HasForeignKey(b => b.UserId).OnDelete(DeleteBehavior.Cascade).IsRequired();

        });
    }
}
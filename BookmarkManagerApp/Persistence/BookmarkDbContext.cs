
using BookmarkManagerApp.Models;
using BookmarkManagerApp.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BookmarkManagerApp.Persistence;

public class BookmarkDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public BookmarkDbContext(DbContextOptions options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("bookmark-manager");
        modelBuilder.UseIdentityByDefaultColumns();
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookmarkDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(new CreationTimeInterceptor(), new LastModifiedTimeInterceptor());
    }
}
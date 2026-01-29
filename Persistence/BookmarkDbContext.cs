
using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Persistence;

public class BookmarkDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Bookmark> Bookmarks { get; set; }
    public DbSet<Visit> Visits { get; set; }
    public DbSet<Tag> Tags { get; set; }
    public DbSet<BookmarkTag> BookmarkTags { get; set; }

    public BookmarkDbContext(DbContextOptions options) : base(options) { } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        modelBuilder.HasDefaultSchema("bookmark_manager");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(BookmarkDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }
}
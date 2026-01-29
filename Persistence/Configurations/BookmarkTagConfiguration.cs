using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookmark_manager_app.Persistence.Configuration;

public class BookmarkTagConfiguration : IEntityTypeConfiguration<BookmarkTag>
{
    public void Configure(EntityTypeBuilder<BookmarkTag> builder)
    {
        builder.ToTable("bookmark_tags");
        builder.HasKey(e => new { e.BookmarkId, e.TagId });

        builder.HasOne(e => e.Bookmark)
           .WithMany(e => e.BookmarkTags)
           .HasForeignKey(e => e.BookmarkId)
           .OnDelete(DeleteBehavior.Cascade);
        builder.HasOne(e => e.Tag)
            .WithMany(e => e.BookmarkTags)
            .HasForeignKey(e => e.TagId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
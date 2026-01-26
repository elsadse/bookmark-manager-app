using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookmark_manager_app.Persistence.Configuration;

public class BookmarkConfiguration : IEntityTypeConfiguration<Bookmark>
{
    public void Configure(EntityTypeBuilder<Bookmark> builder)
    {
        builder.ToTable("bookmarks");
        builder.HasKey(e => e.BookmarkId);
        builder.Property(e => e.BookmarkId).ValueGeneratedOnAdd().UseIdentityColumn();

        builder.HasIndex(e => new { e.UserId, e.Title }).IsUnique();
        builder.HasIndex(e => new { e.UserId, e.Url }).IsUnique();

        builder.Property(e => e.Title).IsRequired();
        builder.Property(e => e.Description).IsRequired().HasMaxLength(280);
        builder.Property(e => e.Url).IsRequired();
        builder.Property(e => e.IsPinned).HasDefaultValue(false);
        builder.Property(e => e.IsArchived).HasDefaultValue(false);
        builder.Property(e => e.CreatedAt).HasDefaultValueSql("NOW()");
        builder.Property(e => e.UpdatedAt).ValueGeneratedOnAddOrUpdate().HasDefaultValueSql("NOW()");

        builder.HasOne(e => e.User).WithMany(e => e.Bookmarks).HasForeignKey(e => e.UserId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.Visits).WithOne(e => e.Bookmark).HasForeignKey(e => e.BookmarkId).OnDelete(DeleteBehavior.Cascade);
        builder.HasMany(e => e.BookmarkTags).WithOne(e => e.Bookmark).HasForeignKey(e => e.BookmarkId).OnDelete(DeleteBehavior.Cascade);
    }
}
using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookmark_manager_app.Persistence.Configuration;

public class VisitConfiguration : IEntityTypeConfiguration<Visit>
{
    public void Configure(EntityTypeBuilder<Visit> builder)
    {
        builder.ToTable("visits");
        builder.HasKey(e => e.VisitId);
        builder.Property(e => e.VisitId).ValueGeneratedOnAdd().UseIdentityColumn();

        builder.HasIndex(e => e.BookmarkId);
        builder.HasIndex(e => e.VisitDateAt);

        builder.Property(e => e.VisitDateAt).HasDefaultValueSql("NOW()");

        builder.HasOne(e => e.Bookmark)
            .WithMany(e => e.Visits)
            .HasForeignKey(e => e.BookmarkId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
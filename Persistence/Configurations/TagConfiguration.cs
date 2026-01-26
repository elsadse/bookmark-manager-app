using bookmark_manager_app.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace bookmark_manager_app.Persistence.Configuration;

public class TagConfiguration : IEntityTypeConfiguration<Tag>
{
    public void Configure(EntityTypeBuilder<Tag> builder)
    {
        builder.ToTable("tags");
        builder.HasKey(e => e.TagId);
         builder.Property(e => e.TagId).ValueGeneratedOnAdd().UseIdentityColumn(); 

        builder.HasIndex(e => e.Name).IsUnique();

        builder.Property(e => e.Name).IsRequired().HasMaxLength(25);

        builder.HasMany(e => e.BookmarkTags).WithOne(e => e.Tag).HasForeignKey(e => e.TagId).OnDelete(DeleteBehavior.Cascade);
    }
}
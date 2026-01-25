using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookmark_manager_app.Models;

[Table("tags", Schema = "bookmark")]
public class Tag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("tag_id")]
    public int TagId { get; set; }

    [Required]
    [Column("name")]
    public string Name { get; set; } = string.Empty;

    public ICollection<BookmarkTag> BookmarkTags { get; set; } = new List<BookmarkTag>();
}

public class TagCreateDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
}
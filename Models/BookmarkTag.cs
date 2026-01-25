using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookmark_manager_app.Models;

[Table("bookmarktags", Schema = "bookmark")]
public class BookmarkTag
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int BookmarkTagId { get; set; }

    [Required]
    [ForeignKey("Bookmark")]
    public int BookmarkId { get; set; }

    [Required]
    [ForeignKey("Tag")]
    public int TagId { get; set; }
    public Bookmark? Bookmark { get; set; }
    public Tag? Tag { get; set; }
}
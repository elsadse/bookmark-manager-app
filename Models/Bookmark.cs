using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookmark_manager_app.Models;

[Table("bookmarks", Schema = "bookmark")]
public class Bookmark
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required]
    [ForeignKey("User")]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("title")]
    public string? Title { get; set; }

    [Required]
    [Url]
    [Column("url")]
    public string? Url { get; set; }

    [Required]
    [Column("description")]
    public string? Description { get; set; }

    [Column("tags", TypeName = "text[]")]
    public List<string> Tags { get; set; } = new List<string>();

    [Column("is_pinned")]
    public bool IsPinned { get; set; } = false;

    [Column("is_archived")]
    public bool IsArchived { get; set; } = false;

    [Column("visit_count")]
    public int VisitCount { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("visited_last_at")]
    public DateTime VisitedLastAt { get; set; } = DateTime.UtcNow;
}

public class BookmarkCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    public string? Description { get; set; }

    public List<string> Tags { get; set; } = new List<string>();
}
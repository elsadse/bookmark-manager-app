using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookmark_manager_app.Models;

[Table("bookmarks", Schema = "bookmark")]
public class Bookmark
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("bookmark_id")]
    public int BookmarkId { get; set; }

    [Required]
    [ForeignKey("User")]
    [Column("user_id")]
    public int UserId { get; set; }

    [Required]
    [Column("title")]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Url]
    [Column("url")]
    public string Url { get; set; } = string.Empty;

    [Required]
    [Column("description")]
    public string Description { get; set; } = string.Empty;

    [Column("is_pinned")]
    public bool IsPinned { get; set; } = false;

    [Column("is_archived")]
    public bool IsArchived { get; set; } = false;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("update_at")]
    public DateTime UpdateAt { get; set; }

    public User? User { get; set; }

    public ICollection<Visit> Visits { get; set; } = new List<Visit>();

    public ICollection<BookmarkTag> BookmarkTags { get; set; } = new List<BookmarkTag>();
}

public class BookmarkCreateDto
{
    [Required]
    public string Title { get; set; } = string.Empty;

    [Required]
    [Url]
    public string Url { get; set; } = string.Empty;

    [Required]
    public string Description { get; set; } = string.Empty;

    [Required]
    public ICollection<int> TagIds { get; set; } = new List<int>();
}

public class BookmarkUpdateDto
{
    public string? Title { get; set; }

    [Url]
    public string? Url { get; set; }

    public string? Description { get; set; }

    public DateTime UpdateAt { get; set; } = DateTime.UtcNow;

    public List<int>? TagIds { get; set; }
}

public class BookmarkPatchDto
{
    public bool? IsPinned { get; set; }

    public bool? IsArchived { get; set; }
}
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookmark_manager_app.Models;

[Table("visits", Schema = "bookmark")]
public class Visit
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("visit_id")]
    public int VisitId { get; set; }

    [Required]
    [ForeignKey("Bookmark")]
    [Column("bookmark_id")]
    public int BookmarkId { get; set; }

    [Column("visit_date_at")]
    public DateTime VisitDateAt { get; set; }

    public Bookmark? Bookmark { get; set; }
}

public class VisitCreateDto
{
    [Required]
    public int BookmarkId { get; set; }
}
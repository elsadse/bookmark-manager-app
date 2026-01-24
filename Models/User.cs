using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace bookmark_manager_app.Models;

[Table("users", Schema = "bookmark")]
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    public string Username { get; set; }= string.Empty;

    [Required]
    [EmailAddress]
    [Column("email")]
    public string Email { get; set; }= string.Empty;

    [Required]
    [Column("password_hash")]
    public string PasswordHash { get; set; }= string.Empty;

}

public class UserCreateDto
{
    [Required]
    public string Username { get; set; }= string.Empty;

    [Required]
    [EmailAddress]
    public string Email { get; set; }= string.Empty;

    [Required]
    public string Password { get; set; }= string.Empty;
}

public class UserUpdateDto
{
    [EmailAddress]
    public string Email { get; set; }= string.Empty;

    public string Password { get; set; }= string.Empty;
}
using System.ComponentModel.DataAnnotations;
using bookmark_manager_app.DTOs;

namespace bookmark_manager_app.Models;

public sealed class User
{
    public int UserId { get; private set; }
    public string FullName { get; private set; }
    public string Email { get; private set; }
    public string PasswordHash { get; private set; }
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }
    private readonly List<Bookmark> _bookmarks = new();
    public IReadOnlyCollection<Bookmark> Bookmarks => _bookmarks.AsReadOnly();

    private User()
    {
        FullName = string.Empty;
        Email = string.Empty;
        PasswordHash = string.Empty;
    }

    private User(string fullName, string email, string passwordHash)
    {
        FullName = fullName;
        Email = email;
        PasswordHash = passwordHash;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = CreatedAt;
    }

    public static User Create(UserCreateDto dto)
    {
        ValidateCreateInputs(dto);
        return new User(
            fullName: dto.FullName,
            email: dto.Email,
            passwordHash: BCrypt.Net.BCrypt.HashPassword(dto.Password, BCrypt.Net.BCrypt.GenerateSalt(12))
        );
    }

    public void Update(UserUpdateDto dto)
    {
        ValidateUpdateInputs(dto);
        bool hasChanges = false;
        if (!string.IsNullOrWhiteSpace(dto.Email) && dto.Email.ToLower() != Email.ToLower())
        {
            Email = dto.Email.ToLower();
            hasChanges = true;
        }
        if (!string.IsNullOrWhiteSpace(dto.Password))
        {
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password, BCrypt.Net.BCrypt.GenerateSalt(12));
        }
        if (hasChanges)
        {
            UpdatedAt = DateTime.UtcNow;
        }
    }

    private static void ValidateCreateInputs(UserCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new ArgumentException("Full name is required", nameof(dto.FullName));
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required", nameof(dto.Email));
        if (!new EmailAddressAttribute().IsValid(dto.Email))
            throw new ArgumentException("Invalid email format", nameof(dto.Email));
        if (string.IsNullOrWhiteSpace(dto.Password))
            throw new ArgumentException("Password is required", nameof(dto.Password));
    }

    private static void ValidateUpdateInputs(UserUpdateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Email))
            throw new ArgumentException("Email is required", nameof(dto.Email));
        if (!new EmailAddressAttribute().IsValid(dto.Email))
            throw new ArgumentException("Please enter a valid email address", nameof(dto.Email));
    }

}



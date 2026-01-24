using System.Security.Cryptography;
using bookmark_manager_app.Data;
using bookmark_manager_app.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Services;

public class UserService : IUserService
{
    private readonly BookmarkDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(BookmarkDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    private string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);
        string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
            password: password,
            salt: salt,
            prf: KeyDerivationPrf.HMACSHA256,
            iterationCount: 100000,
            numBytesRequested: 256 / 8));

        return hashed;
    }

    public async Task<User?> CreateUserAsync(UserCreateDto userDto)
    {
        try
        {
            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == userDto.Email);
            if (existingUser != null)
            {
                _logger.LogWarning("Email {Email} already exists", userDto.Email);
                return null;
            }
            var user = new User
            {
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = HashPassword(userDto.Password),
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            _logger.LogInformation("User created with ID: {UserId}", user.Id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating user with email {Email}", userDto.Email);
            return null;
        }
    }

    public async Task<User?> GetUserByIdAsync(int id)
    {
        try
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id);
            return user;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting user with ID: {UserId}", id);
            return null;
        }
    }

    public async Task<bool> UpdateUserAsync(int id, UserUpdateDto userUpdate)
    {
        try
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                _logger.LogWarning("User with ID {UserId} not found", id);
                return false;
            }
            bool hasChanges = false;
            if (!string.IsNullOrEmpty(userUpdate.Password))
            {
                user.PasswordHash = HashPassword(userUpdate.Password);
                hasChanges = true;
            }
            if (hasChanges)
            {
                await _context.SaveChangesAsync();
                _logger.LogInformation("User with ID {UserId} updated successfully", id);
                return true;
            }
            _logger.LogInformation("No changes to update for user ID {UserId}", id);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating user with ID: {UserId}", id);
            return false;
        }
    }

}
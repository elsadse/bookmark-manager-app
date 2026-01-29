using bookmark_manager_app.DTOs;
using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class UserRepository : IUserRepository
{
    private readonly BookmarkDbContext _dbContext;

    public UserRepository(BookmarkDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> GetByIdAsync(int userId)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == userId);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        if (user is null) return null;
        return user;
    }

    public async Task<UserDto> CreateAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return new UserDto
        {
            Email = user.Email,
            FullName = user.FullName,
        };
    }

    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await GetByIdAsync(userId);
        if (user != null)
        {
            _dbContext.Users.Remove(user);
            await _dbContext.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int userId)
    {
        return await _dbContext.Users.AnyAsync(u => u.UserId == userId);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
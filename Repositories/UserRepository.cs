using bookmark_manager_app.Models;
using bookmark_manager_app.Persistence;
using Microsoft.EntityFrameworkCore;

namespace bookmark_manager_app.Repositories;

public class UserRepository(BookmarkDbContext context)
{
   public async Task<User?> GetByEmailAsync(string email)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
        if (user is null) return null;
        return user;
    }

    public async Task<User?> GetByIdAsync(long userId)
    {
        var user = await context.Users.FirstOrDefaultAsync(u => u.UserId==userId);
        if (user is null) return null;
        return user;
    }

    public async Task<User> CreateAsync(User user)
    {
        await context.Users.AddAsync(user);
        await context.SaveChangesAsync();
        return user;
    }

    public async Task UpdateAsync(User user)
    {
        context.Users.Update(user);
        await context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int userId)
    {
        var user = await GetByIdAsync(userId);
        if (user != null)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await context.Users.AsNoTracking().AnyAsync(u => u.Email.ToLower() == email.ToLower());
    }
}
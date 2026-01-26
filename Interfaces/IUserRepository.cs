using bookmark_manager_app.DTOs;
using bookmark_manager_app.Models;

namespace bookmark_manager_app.Interfaces;

public interface IUserRepository
{
    Task<User?> GetByIdAsync(int userId);
    Task<User?> GetByEmailAsync(string email);
    Task<UserDto> CreateAsync(User user);
    Task UpdateAsync(User user);
    Task DeleteAsync(int userId);
    Task<bool> ExistsAsync(int userId);
    Task<bool> EmailExistsAsync(string email);
}
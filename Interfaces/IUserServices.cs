using bookmark_manager_app.DTOs;
using bookmark_manager_app.Models;

namespace bookmark_manager_app.Interfaces;

public interface IUserService
{
    Task<UserDto> CreateUserAsync(UserCreateDto command);
    Task<User?> GetUserByIdAsync(int id);
    Task UpdateUserAsync(int id, UserUpdateDto command);
}
using bookmark_manager_app.Models;

namespace bookmark_manager_app.Services;

public interface IUserService
{
    Task<User?> CreateUserAsync(UserCreateDto userDto);
    Task<User?> GetUserByIdAsync(int userId);
    Task<bool> UpdateUserAsync(int id, UserUpdateDto userUpdate);
}
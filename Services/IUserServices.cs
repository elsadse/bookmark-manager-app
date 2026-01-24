using bookmark_manager_app.Models;

namespace bookmark_manager_app.Services;

public interface IUserService
{
    Task<User?> CreateUserAsync(UserCreateDto user);
    Task<User?> GetUserByIdAsync(int id);
    Task<bool> UpdateUserAsync(int id, UserUpdateDto user);

}
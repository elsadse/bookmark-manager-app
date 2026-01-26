using bookmark_manager_app.DTOs;
using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Interfaces;
using bookmark_manager_app.Models;

namespace bookmark_manager_app.Services;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> CreateUserAsync(UserCreateDto command)
    {
        var existingUser = await _userRepository.GetByEmailAsync(command.Email);
        if (existingUser != null)
            throw new ConflictException($"User with email '{command.Email}' already exists");
        var user = User.Create(command);
        var createdUser = await _userRepository.CreateAsync(user);
        return createdUser;
    }

    public async Task<User?> GetUserByIdAsync(int userId)
    {
        var user = await _userRepository.GetByIdAsync(userId);
        if (user is null)
            throw new NotFoundException("User ID not found");
        return user;

    }

    public async Task UpdateUserAsync(int id, UserUpdateDto command)
    {
        var user = await _userRepository.GetByEmailAsync(command.Email);
        if (user is null)
            throw new NotFoundException($"User with email doesn't exist");
        if (user.UserId != id)
            throw new ForbiddenException($"You cannot access this resource.");
        user.Update(command);
        await _userRepository.UpdateAsync(user);
    }

}
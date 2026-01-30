using bookmark_manager_app.Exceptions;
using bookmark_manager_app.Models;
using bookmark_manager_app.Repositories;
using Microsoft.AspNetCore.Identity;

namespace bookmark_manager_app.Services;

public class AuthService(UserRepository userRepository, PasswordHasher<IdentityUser> passwordHasher)
{

    public async Task<User> RegisterAsync(string fullname, string email, string password)
    {
        if (await userRepository.GetByEmailAsync(email) is not null)
            throw new ConflictException("Email already exists");
        var hashedPassword = await HashPassword(password);
        return await userRepository.CreateAsync(new User
        { Email = email, Fullname = fullname, Password = hashedPassword });
    }

    private async Task<string> HashPassword(string password) =>
        await Task.FromResult(passwordHasher.HashPassword(new IdentityUser(), password));

}
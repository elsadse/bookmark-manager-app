using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BookmarkManagerApp.Services.Utils;

namespace BookmarkManagerApp.UnitTests.Services.Utils;

public class UserContextTests
{
    [Fact]
    public void Constructor_Should_Set_UserId_When_Claim_Is_Present()
    {
        // Arrange
        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, "42")
        };

        var identity = new ClaimsIdentity(claims);
        var principal = new ClaimsPrincipal(identity);

        // Act
        var userContext = new UserContext(principal);

        // Assert
        Assert.Equal(42, userContext.UserId);
    }

    [Fact]
    public void Constructor_Should_Throw_When_UserId_Claim_Missing()
    {
        // Arrange
        var identity = new ClaimsIdentity();
        var principal = new ClaimsPrincipal(identity);

        // Act & Assert
        Assert.Throws<UnauthorizedAccessException>(() => new UserContext(principal));
    }
}
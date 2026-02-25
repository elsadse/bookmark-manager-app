using System.Security.Claims;
using BookmarkManagerApp.Services.Contracts;

namespace BookmarkManagerApp.Services.Utils;

public class UserContext : IUserContext
{
    public long UserId { get; }

    public UserContext(ClaimsPrincipal principal)
    {
        UserId = JwtClaimGetter
            .TryGetUserIdFromClaimsPrincipalOrElseThrow(principal);
    }
}
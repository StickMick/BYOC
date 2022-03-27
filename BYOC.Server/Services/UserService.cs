using Microsoft.AspNetCore.Identity;

namespace BYOC.Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<IdentityUser> _userManager;

    public UserService(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IdentityUser> FindByApiKey(string key)
    {
        return new IdentityUser();
    }
}
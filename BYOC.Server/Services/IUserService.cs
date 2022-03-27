using Microsoft.AspNetCore.Identity;

namespace BYOC.Server.Services;

public interface IUserService
{
    Task<IdentityUser> FindByApiKey(string key);
}
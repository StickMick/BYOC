using System.Security.Claims;
using BYOC.Server.Data.Entities;
using BYOC.Shared;
using Microsoft.AspNetCore.Identity;

namespace BYOC.Server.Services;

public interface IUserService
{
    Task<List<ApplicationUser>> GetListAsync(CancellationToken token = default);
    Task<ApplicationUser?> FindByApiKey(string key, CancellationToken token = default);
    Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken token = default);
    Task<Result> CreateUser(ApplicationUser user, string password);
    Task<string> CreateApiKey(Guid userId, CancellationToken token = default);
    Task<ApplicationUser?> GetByEmailAsync(string userEmail, CancellationToken token = default);
}
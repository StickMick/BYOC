using System.Security.Claims;
using BYOC.Server.Data;
using BYOC.Server.Data.Entities;
using BYOC.Shared;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BYOC.Server.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;

    public UserService(
        UserManager<ApplicationUser> userManager,
        ApplicationDbContext context)
    {
        _userManager = userManager;
        _context = context;
    }

    public Task<List<ApplicationUser>> GetListAsync(CancellationToken token = default)
    {
        return _context.Users.ToListAsync(token);
    }

    public Task<ApplicationUser?> FindByApiKey(string key, CancellationToken token = default)
    {
        return _context.Users.Where(u=>
            u.ApiKeys.Any(k=>k.Key == key))
            .FirstOrDefaultAsync(token);
    }

    public Task<IList<Claim>> GetClaimsAsync(ApplicationUser user, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
        {
            return Task.FromCanceled<IList<Claim>>(token);
        }
        return _userManager.GetClaimsAsync(user);
    }

    public async Task<Result> CreateUser(ApplicationUser user, string password)
    {
        var result = await _userManager.CreateAsync(user, password);
        if (result.Succeeded)
        {
            return new SuccessResult();
        }
        var errors = result.Errors.Select(e => e.Description);
        return new ErrorResult(result.Errors.Select(e => e.Description));
    }
    
    public async Task<string> CreateApiKey(Guid userId, CancellationToken token = default)
    {
        var key = Guid.NewGuid().ToString();
        var apiKey = new ApiKey
        {
            Key = Guid.NewGuid().ToString(),
            IsActive = true,
            CreatedDate = DateTime.Now,
            UserId = userId
        };
        _context.ApiKeys.Add(apiKey);
        await _context.SaveChangesAsync(token);
        return key;
    }

    public Task<ApplicationUser?> GetByEmailAsync(string userEmail, CancellationToken token = default)
    {
        return _context.Users.SingleOrDefaultAsync(u => u.Email == userEmail, token);
    }
}
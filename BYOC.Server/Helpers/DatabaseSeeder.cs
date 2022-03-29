using BYOC.Server.Data;
using BYOC.Server.Data.Entities;
using BYOC.Server.Services;
using Microsoft.EntityFrameworkCore;

namespace BYOC.Server.Helpers;

public class DatabaseSeeder
{
    private readonly ApplicationDbContext _context;
    private readonly IUserService _userService;

    public DatabaseSeeder(
        ApplicationDbContext context,
        IUserService userService)
    {
        _context = context;
        _userService = userService;
    }

    public Task MigrateAsync()
    {
        return _context.Database.MigrateAsync();
    }
    
    public async Task SeedAsync()
    {
        if ((await _userService.GetListAsync()).Any())
            return;

        var user = new ApplicationUser
        {
            UserName = "test@example.com",
            Email = "test@example.com"
        };
        
        await _userService.CreateUser(user, "Password1!");
        user = await _userService.GetByEmailAsync(user.Email);
        await _userService.CreateApiKey(user.Id);
    }
}
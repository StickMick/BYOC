using Microsoft.AspNetCore.Identity;

namespace BYOC.Server.Data.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public List<ApiKey> ApiKeys { get; set; }
}
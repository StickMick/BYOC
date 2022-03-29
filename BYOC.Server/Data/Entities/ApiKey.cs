namespace BYOC.Server.Data.Entities;

public class ApiKey
{
    public Guid Id { get; set; }
    public string Key { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? LastModifiedDate { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
}
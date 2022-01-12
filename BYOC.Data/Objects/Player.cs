namespace BYOC.Data.Objects;

public class Player
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public BasicList<Unit?> Units { get; set; } = new();
}
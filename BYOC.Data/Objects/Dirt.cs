namespace BYOC.Data.Objects;
public record Dirt() : IInteractable
{
    public BasicList<EnumActions> Actions => new()
    {
        EnumActions.Walk
    };
}
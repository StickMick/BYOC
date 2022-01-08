namespace BYOC.Data.Objects;
public record Grass() : IInteractable
{
    public BasicList<EnumActions> Actions => new()
    {
        EnumActions.Walk
    };
}
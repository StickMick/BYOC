namespace BYOC.Data.Objects;
public record Rock() : IInteractable
{
    public BasicList<EnumActions> Actions => new()
    {
        EnumActions.Harvest
    };
}
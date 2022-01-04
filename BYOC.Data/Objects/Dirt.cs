namespace BYOC.Data.Objects;

public record Dirt() : IInteractable
{
    public Actions Actions => Actions.Walk;
};
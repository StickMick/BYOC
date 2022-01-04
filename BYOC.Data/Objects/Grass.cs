namespace BYOC.Data.Objects;

public record Grass() : IInteractable
{
    public Actions Actions => Actions.Walk;
};
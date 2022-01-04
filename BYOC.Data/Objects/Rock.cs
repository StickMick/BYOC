namespace BYOC.Data.Objects;

public record Rock() : IInteractable
{
    public Actions Actions => Actions.Harvest;
};
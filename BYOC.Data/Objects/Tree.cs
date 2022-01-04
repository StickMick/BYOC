namespace BYOC.Data.Objects;

public record Tree() : IInteractable
{
    public Actions Actions => Actions.Harvest | Actions.Attack;
};
namespace BYOC.Data.Objects;

public record Tree() : IInteractable
{

    //public Actions Actions => Actions.Harvest | Actions.Attack;
    public BasicList<EnumActions> Actions => new()
    {
        EnumActions.Harvest,
        EnumActions.Attack
    };
}
namespace BYOC.Data.Objects;
public interface IInteractable
{
    BasicList<EnumActions> Actions { get; } //so more than one can be chosen.
    //EnumActions Actions { get; }
}
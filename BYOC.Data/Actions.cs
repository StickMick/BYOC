namespace BYOC.Data;

[Flags]
public enum Actions
{
    None     = 1 << 0,
    Move     = 1 << 1,
    Walk     = 1 << 2,
    Attack   = 1 << 3,
    Harvest  = 1 << 4,
    PickUp   = 1 << 5,
}
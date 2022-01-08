namespace BYOC.Data;
public readonly partial record struct EnumActions
{
    private enum EnumInfo
    {
        None,
        Move,
        Walk,
        Attack,
        Harvest,
        PickUp
    }
}
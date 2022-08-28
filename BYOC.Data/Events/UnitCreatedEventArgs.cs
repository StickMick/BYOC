using BYOC.Data.Objects;

namespace BYOC.Data.Events;

public class UnitCreatedEventArgs : EventArgs
{
    public Unit Unit { get; set; }
}
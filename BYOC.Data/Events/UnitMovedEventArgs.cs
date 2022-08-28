using BYOC.Data.Objects;

namespace BYOC.Data.Events;

public class UnitMovedEventArgs : EventArgs
{
    public Guid Id { get; set; }
    public Position Position { get; set; }
}
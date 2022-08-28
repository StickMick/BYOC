using BYOC.Data.Events;
using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public interface IUnitRepository
{
    BasicList<Unit> Units { get; }
    EventHandler<UnitCreatedEventArgs> OnUnitCreated { get; set; }
    Unit? GetUnit(Guid Id);
    Unit? AddUnit(Guid playerId, int x, int y);
}
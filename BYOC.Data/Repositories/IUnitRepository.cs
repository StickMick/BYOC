using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public interface IUnitRepository
{
    BasicList<Unit> Units { get; }
    Unit? GetUnit(Guid Id);
    Unit? AddUnit(Guid playerId, int x, int y);
}
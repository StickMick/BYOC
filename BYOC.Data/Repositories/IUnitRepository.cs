using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public interface IUnitRepository
{
    BasicList<Unit> Units { get; set; }
    Unit? GetUnit(int x, int y);
}
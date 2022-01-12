using BYOC.Data.Objects;

namespace BYOC.Data.Controllers;

public interface IUnitController
{
    bool TryMoveUnit(Guid id, int x, int y);
}
using BYOC.Data.Objects;

namespace BYOC.Data.Controllers;

public interface IUnitController
{
    void Tick();
    bool TryMoveUnit(Unit unit, int x, int y);
}
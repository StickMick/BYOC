using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;

namespace BYOC.Data.Controllers;

public class UnitController : IUnitController
{
    private readonly ITileService _tileService;
    private readonly IUnitService _unitService;

    public UnitController(ITileService tileService, IUnitService unitService)
    {
        _tileService = tileService;
        _unitService = unitService;
    }

    public bool TryMoveUnit(Guid id, int x, int y)
    {
        var unit = _unitService.GetUnit(id);
        if (unit is null) return false;
        unit.Path = _tileService.GetPath(unit.Position, new Position(x, y));
        return true;
    }
}
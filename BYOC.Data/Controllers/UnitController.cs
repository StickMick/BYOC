using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;

namespace BYOC.Data.Controllers;

public class UnitController : IUnitController
{
    private readonly ITileRepository _tileRepository;
    private readonly IUnitRepository _unitRepository;

    public UnitController(ITileRepository tileRepository, IUnitRepository unitRepository)
    {
        _tileRepository = tileRepository;
        _unitRepository = unitRepository;
    }

    public void Tick()
    {
        foreach (var unit in _unitRepository.Units)
        {
            unit.Tick();
        }
    }

    public bool TryMoveUnit(Unit unit, int x, int y)
    {
        var tile = _tileRepository.GetTile(x, y);
        if (tile!.IsWalkable)
        {
            unit.SetMoveTarget(tile.Position);
            Console.WriteLine($"Moved to {x},{y}");
            return true;
        }
        Console.WriteLine($"Tried to move to [{x},{y}] but was blocked");
        return false;
    }
}
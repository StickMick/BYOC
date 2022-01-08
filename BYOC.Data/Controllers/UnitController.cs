using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;

namespace BYOC.Data.Controllers;

public class UnitController
{
    public List<Unit> Units { get; set; } = new List<Unit>();
    
    private readonly TileRepository _tileRepository;

    public UnitController(TileRepository tileRepository)
    {
        _tileRepository = tileRepository;
    }

    public void Tick()
    {
        foreach (var unit in Units)
        {
            unit.Tick();
        }
    }

    public bool TryMoveUnit(Unit unit, int x, int y)
    {
        var tile = _tileRepository.GetTile(x, y);
        if (tile.IsWalkable)
        {
            unit.SetMoveTarget(tile.Position);
            Console.WriteLine($"Moved to {x},{y}");
            return true;
        }
        Console.WriteLine($"Tried to move to [{x},{y}] but was blocked");
        return false;
    }
}
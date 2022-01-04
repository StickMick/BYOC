using BYOC.Data.Objects;
using BYOC.Data.Services;

namespace BYOC.Data.Controllers;

public class UnitController
{
    private readonly WorldService _worldService;

    public UnitController(WorldService worldService)
    {
        _worldService = worldService;
    }
    
    public List<Unit> Units { get; set; }

    public bool TryMoveUnit(Unit unit, int x, int y)
    {
        var tiles = _worldService.GetTile(x, y);
        if (tiles.All(t => t.Actions.HasFlag(Actions.Walk)))
        {
            unit.Move(x,y);
            Console.WriteLine($"Moved to {x},{y}");
            return true;
        }
        Console.WriteLine($"Tried to move to [{x},{y}] but was blocked");
        return false;
    }
}
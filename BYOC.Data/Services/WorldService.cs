using BYOC.Data.Objects;
using BYOC.Data.Repositories;

namespace BYOC.Data.Services;

public class WorldService
{
    private readonly World _world;
    private readonly TileRepository _tileRepository;
    private readonly UnitRepository _unitRepository;

    public WorldService(
        World world, 
        TileRepository tileRepository,
        UnitRepository unitRepository)
    {
        _world = world;
        _tileRepository = tileRepository;
        _unitRepository = unitRepository;
    }

    public List<IInteractable> GetTile(int x, int y)
    {
        var results = new List<IInteractable>();
        
        results.Add(_tileRepository.Data[_world.Tiles[x, y]]);

        var unit = _unitRepository.GetUnit(x, y);
        
        if (unit != null)
            results.Add(unit);

        return results;
    }
}
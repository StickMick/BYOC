using BYOC.Data.Objects;
using BYOC.Data.Repositories;

namespace BYOC.Data.Services;

public class WorldService
{
    private readonly TileRepository _tileRepository;
    private readonly UnitRepository _unitRepository;

    public WorldService(
        TileRepository tileRepository,
        UnitRepository unitRepository)
    {
        _tileRepository = tileRepository;
        _unitRepository = unitRepository;
    }
    
}
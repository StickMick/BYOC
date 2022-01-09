using BYOC.Data.Objects;
using BYOC.Data.Repositories;

namespace BYOC.Data.Services;

public class WorldService : IWorldService
{
    private readonly ITileRepository _tileRepository;
    private readonly IUnitRepository _unitRepository;

    public WorldService(
        ITileRepository tileRepository,
        IUnitRepository unitRepository)
    {
        _tileRepository = tileRepository;
        _unitRepository = unitRepository;
    }
    
    
    
}
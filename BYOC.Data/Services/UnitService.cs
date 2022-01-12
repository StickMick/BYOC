using BYOC.Data.Objects;
using BYOC.Data.Repositories;

namespace BYOC.Data.Services;

public class UnitService : IUnitService
{
    private readonly IUnitRepository _unitRepository;
    private readonly ITileRepository _tileRepository;
    private readonly ISessionService _sessionService;

    public UnitService(IUnitRepository unitRepository, ITileRepository tileRepository, ISessionService sessionService)
    {
        _unitRepository = unitRepository;
        _tileRepository = tileRepository;
        _sessionService = sessionService;
    }


    public Unit? GetUnit(Guid Id)
    {
        var unit = _unitRepository.GetUnit(Id);
        if (unit?.Player.Id != _sessionService.GetPlayerId()) return null;
        return unit;
    }
}

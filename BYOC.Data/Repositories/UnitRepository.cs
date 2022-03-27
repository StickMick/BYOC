using BYOC.Data.Objects;
using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.BasicExtensions;
using Serilog;

namespace BYOC.Data.Repositories;

public class UnitRepository : IUnitRepository
{
    private readonly IWorld _world;
    private readonly ILogger _logger;

    public UnitRepository(IWorld world, ILogger logger)
    {
        _world = world;
        _logger = logger;
    }

    public BasicList<Unit> Units => _world.Players.SelectMany(p => p.Units).ToBasicList();

    public Unit? GetUnit(Guid Id) => Units.FirstOrDefault(u => u.Id == Id);
    public Unit? AddUnit(Guid playerId, int x, int y)
    {
        var unit = new Unit(_logger, x, y);
        var player = _world.Players.FirstOrDefault(p => p.Id == playerId);
        if (player != null)
        {
            unit.Player = player;
            player.Units.Add(unit);
            return unit;
        }

        return null;
    }
}
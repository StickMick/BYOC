using BYOC.Data.Objects;
using BYOC.Data.Repositories;

namespace BYOC.Data.Services;

public class WorldService : IWorldService
{
    private readonly ITileRepository _tileRepository;
    private readonly IUnitRepository _unitRepository;
    private readonly IWorld _world;

    public WorldService(
        ITileRepository tileRepository,
        IUnitRepository unitRepository,
        IWorld world)
    {
        _tileRepository = tileRepository;
        _unitRepository = unitRepository;
        _world = world;
    }


    public BasicList<Player> GetPlayers()
    {
        return _world.Players;
    }

    public Player GetPlayer(Guid Id)
    {
        return _world.Players.SingleOrDefault(p => p.Id == Id);
    }

    public Player AddPlayer(Player player)
    {
        _world.Players.Add(player);
        return player;
    }

    public void RemovePlayer(Player player)
    {
        _world.Players.RemoveSpecificItem(player);
    }

    public void RemovePlayer(Guid Id)
    {
        _world.Players.RemoveAllOnly(p => p.Id == Id);
    }
}
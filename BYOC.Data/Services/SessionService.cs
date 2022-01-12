using BYOC.Data.Objects;

namespace BYOC.Data.Services;

public class SessionService : ISessionService
{
    private readonly IWorld _world;

    public SessionService(IWorld world)
    {
        _world = world;
    }

    public Guid GetPlayerId()
    {
        return _world.Players.Select(p => p.Id).First();
    }

    public Player GetPlayer()
    {
        return _world.Players.First();
    }
}
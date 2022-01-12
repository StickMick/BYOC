using BYOC.Data.Objects;

namespace BYOC.Data.Services;

public interface IWorldService
{
    BasicList<Player> GetPlayers();
    Player GetPlayer(Guid Id);
    Player AddPlayer(Player player);
    void RemovePlayer(Player player);
    void RemovePlayer(Guid Id);
}
using BYOC.Data.Models;

namespace BYOC.Data.Controllers;

public interface IGameController
{
    Task Start(GameOptions options, CancellationToken token = default);
    Task Start(Action<GameOptions> options, CancellationToken token = default);
}
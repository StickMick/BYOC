using BYOC.Data.Models;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;

namespace BYOC.Data.Controllers;

public class GameController : IGameController
{
    private readonly IUnitRepository _unitRepository;
    private readonly IWorldService _worldService;

    public GameController(
        IUnitRepository unitRepository,
        IWorldService worldService)
    {
        _unitRepository = unitRepository;
        _worldService = worldService;
    }
    
    public async Task Start(Action<GameOptions> options, CancellationToken token = default)
    {
        var gameOptions = new GameOptions();
        options(gameOptions);

        await Start(gameOptions, token);
    }
    
    public async Task Start(GameOptions options, CancellationToken token = default)
    {
        var periodicTimer = new PeriodicTimer(TimeSpan.FromMilliseconds(options.TickRate));
        
        Setup();
        
        while (await periodicTimer.WaitForNextTickAsync(token))
        {
            if (token.IsCancellationRequested)
            {
                break;
            }
            
            Tick();
        }
    }

    private void Setup()
    {
        
    }

    private void Tick()
    {
        _unitRepository.Units.ForEach(u=>u.Tick());
    }
}
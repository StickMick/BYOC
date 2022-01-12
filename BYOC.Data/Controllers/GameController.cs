using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;

namespace BYOC.Data.Controllers;

public class GameController : IGameController
{
    private readonly IUnitRepository _unitRepository;
    private readonly IWorldService _worldService;

    public GameController(IUnitRepository unitRepository,
        IWorldService worldService)
    {
        _unitRepository = unitRepository;
        _worldService = worldService;
    }
    
    public async Task Start()
    {
        var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        
        

        Setup();
        
        
        while (await periodicTimer.WaitForNextTickAsync())
        {
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
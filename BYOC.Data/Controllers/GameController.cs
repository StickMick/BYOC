using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;

namespace BYOC.Data.Controllers;

public class GameController : IGameController
{
    private readonly IUnitController _unitController;
    private readonly IWorldService _worldService;

    private readonly IUnitRepository _unitRepository;

    public GameController(
        IUnitController unitController,
        IWorldService worldService)
    {
        _unitController = unitController;
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
        _unitController.Tick();
    }
}
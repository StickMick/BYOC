using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;

namespace BYOC.Data.Controllers;

public class GameController
{
    private CancellationToken _token;
    private readonly UnitController _unitController;
    private readonly WorldService _worldService;

    private UnitRepository _unitRepository;

    public GameController(
        CancellationToken token,
        UnitController unitController,
        WorldService worldService)
    {
        _token = token;
        _unitController = unitController;
        _worldService = worldService;
    }
    
    public async Task Start()
    {
        var periodicTimer = new PeriodicTimer(TimeSpan.FromSeconds(1));
        
        

        Setup();
        
        
        while (await periodicTimer.WaitForNextTickAsync(_token))
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
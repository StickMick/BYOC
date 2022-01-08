using BYOC.Data;
using BYOC.Data.Controllers;
using BYOC.Data.Helpers;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;
using CommonBasicLibraries.AdvancedGeneralFunctionsAndProcesses.RandomGenerator;
using BYOC.Data.StateMachines;
World world = new ();

TileRepository tileRepository = new (world);
UnitRepository unitRepository = new ();

WorldService worldService = new (tileRepository, unitRepository);

UnitController unitController = new (tileRepository);

var testUnit = new Unit(4, 4);
unitController.Units.Add(testUnit);
unitController.Units.Add(new Unit(5,5));

CancellationTokenSource tokenSource = new ();
CancellationToken token = tokenSource.Token;

GameController gameController = new (token, unitController, worldService);

TileService tileService = new (tileRepository);

var width = 30;
var height = 30;

tileService.Seed(width,height);

IRandomGenerator random = RandomHelpers.GetRandomGenerator();


//Random random = new Random();

var path = tileService.GetPath(new Position(GetRandomWidth(), GetRandomHeight()), new Position(GetRandomWidth(), GetRandomHeight()));

MapVisualizer.DrawPath(world, path);

//var path = tileService.GetPath(new Position(random.Next(0,width), random.Next(0,height)), new Position(random.Next(0,width), random.Next(0,height)));

await gameController.Start();

int GetRandomWidth()
{
    return random.GetRandomNumber(width) - 1; //because 0 based
}
int GetRandomHeight()
{
    return random.GetRandomNumber(height) - 1; //because 0 based
}

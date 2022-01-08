using BYOC.Data;
using BYOC.Data.Controllers;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;
using BYOC.Data.StateMachines;

World world = new World();

TileRepository tileRepository = new TileRepository(world);
UnitRepository unitRepository = new UnitRepository();

WorldService worldService = new WorldService(tileRepository, unitRepository);

UnitController unitController = new UnitController(tileRepository);

var testUnit = new Unit(4, 4);
unitController.Units.Add(testUnit);
unitController.Units.Add(new Unit(5,5));

CancellationTokenSource tokenSource = new CancellationTokenSource();
CancellationToken token = tokenSource.Token;

GameController gameController = new GameController(token, unitController, worldService);

await gameController.Start();
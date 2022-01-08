using BYOC.Data;
using BYOC.Data.Controllers;
using BYOC.Data.Helpers;
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

TileService tileService = new TileService(tileRepository);

var width = 30;
var height = 30;

tileService.Seed(width,height);

Random random = new Random();

var path = tileService.GetPath(new Position(random.Next(0,width), random.Next(0,height)), new Position(random.Next(0,width), random.Next(0,height)));

MapVisualizer.DrawPath(world, path);



await gameController.Start();
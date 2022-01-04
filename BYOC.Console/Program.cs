using BYOC.Data;
using BYOC.Data.Controllers;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;

World world = new World(10,10);

for (int i = 0; i < world.Width; i++)
{
    for (int j = 0; j < world.Height; j++)
    {
        Console.WriteLine($"[{i},{j}] {world.Tiles[i, j].ToString()}");
    }
}

TileRepository tileRepository = new TileRepository();
UnitRepository unitRepository = new UnitRepository();

var testUnit = new Unit(4, 4);
unitRepository.Units.Add(testUnit);
unitRepository.Units.Add(new Unit(5,5));

WorldService worldService = new WorldService(world, tileRepository, unitRepository);

UnitController unitController = new UnitController(worldService);


unitController.TryMoveUnit(testUnit, 5, 6);
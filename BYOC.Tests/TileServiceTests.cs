using BYOC.Data;
using BYOC.Data.Helpers;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;
using NUnit.Framework;

namespace BYOC.Tests;

public class TileServiceTests
{
    private TileService _tileService;

    private World _world;
    
    [SetUp]
    public void Setup()
    {
        _world = new World();
        
        TileRepository tileRepository = new TileRepository(_world);

        _tileService = new TileService(tileRepository);
    }

    [Test]
    public void CanCreateWorld()
    {
        _tileService.Seed(100,100);
        
        Assert.NotNull(_tileService.GetTile(1,1));
    }
    
    [Test]
    public void InvalidTileReturnsNull()
    {
        _tileService.Seed(100,100);
        
        Assert.IsNull(_tileService.GetTile(-1,-1));
        Assert.IsNull(_tileService.GetTile(-1,100));
        Assert.IsNull(_tileService.GetTile(100,-1));
        Assert.IsNull(_tileService.GetTile(100,101));
    }

    [Test]
    public void CanGetPath()
    {
        _tileService.Seed(50,50);
        var path = _tileService.GetPath(new Position(0, 0), new Position(49, 49));
        Assert.IsNotEmpty(path);
    }
}
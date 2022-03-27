using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public class TileRepository : ITileRepository
{
    private readonly IWorld _world;
    private readonly IRandomGenerator _random;
    public TileRepository(IWorld world)
    {
        _world = world;
        _random = RandomHelpers.GetRandomGenerator();
    }
    
    public Node? GetTile(int x, int y)
    {
        if (x >= 0 && x <= _world.Width - 1 && y >= 0 && y <= _world.Height - 1)
            return _world.Nodes.SingleOrDefault(n=>n.Position.X == x && n.Position.Y == y);
        return null;
    }
    
    public Node? GetTile(Position position)
    {
        return GetTile(position.X, position.Y);
    }
    
    public void Seed(int width, int height)
    {
        _world.Reset(width, height);

        //Array tileTypes = Enum.GetValues(typeof(Tile));
        //now you already have the list because of fast enum.
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Position position = new (i, j);
                bool walkable = _random.NextBool(90); //this means there is a 90 percent chance of being walkable.
                Node node = new(position, walkable);
                _world.Nodes.Add(node);
            }
        }
    }
    
    public IEnumerable<Node> GetWalkableAdjacentSquares(Position position)
    {
        var proposedLocations = new BasicList<Position>()
        {
            new() { X = position.X - 1, Y = position.Y - 1 },
            new() { X = position.X,     Y = position.Y - 1 },
            new() { X = position.X + 1, Y = position.Y - 1 },
            
            new() { X = position.X - 1, Y = position.Y + 1 },
            new() { X = position.X,     Y = position.Y + 1 },
            new() { X = position.X + 1, Y = position.Y + 1 },
            
            new() { X = position.X - 1, Y = position.Y },
            new() { X = position.X + 1, Y = position.Y },
        };

        BasicList<Node?> nodes = new ();

        proposedLocations.ForEach(p => nodes.Add(GetTile(p)));

        nodes.RemoveAllOnly(n => n == null);

        return nodes.Where(n => n!.IsWalkable).AsEnumerable()!;
    }
}
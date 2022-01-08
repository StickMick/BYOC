using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public class TileRepository : ITileRepository
{
    private readonly World _world;

    public TileRepository(World world)
    {
        _world = world;
        
        Data.Add(Tile.Dirt,new Dirt());
        Data.Add(Tile.Grass,new Grass());
        Data.Add(Tile.Rock,new Rock());
        Data.Add(Tile.Tree,new Tree());
    }
    
    public Dictionary<Tile, IInteractable> Data = new Dictionary<Tile, IInteractable>();
    
    public Node? GetTile(int x, int y)
    {
        if (x >= 0 && x <= _world.Width - 1 && y >= 0 && y <= _world.Height - 1)
            return _world.Nodes[x, y];
        return null;
    }
    
    public Node? GetTile(Position position)
    {
        return GetTile(position.X, position.Y);
    }
    
    public void Seed(int width, int height)
    {
        _world.Reset(width, height);

        Array tileTypes = Enum.GetValues(typeof(Tile));
        Random random = new Random();
        
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                Position position = new Position(i, j);
                Node? node = new Node(position, random.Next(0,100) < 90);
                _world.Nodes[i, j] = node;
            }
        }
    }
    
    public IEnumerable<Node> GetWalkableAdjacentSquares(Position position)
    {
        var proposedLocations = new List<Position>()
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

        List<Node?> nodes = new List<Node?>();

        proposedLocations.ForEach(p => nodes.Add(GetTile(p)));

        nodes.RemoveAll(n => n == null);

        return nodes.Where(n => n.IsWalkable).AsEnumerable();
    }
}
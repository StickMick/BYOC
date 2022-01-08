using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public class TileRepository
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
    
    public Node GetTile(int x, int y)
    {
        return _world.Nodes[x, y];
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
                Node node = new Node(position, true);
                _world.Nodes[i, j] = node;
            }
        }
    }
}
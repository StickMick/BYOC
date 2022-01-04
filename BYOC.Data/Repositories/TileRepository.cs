using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public class TileRepository
{
    public TileRepository()
    {
        Data.Add(Tile.Dirt,new Dirt());
        Data.Add(Tile.Grass,new Grass());
        Data.Add(Tile.Rock,new Rock());
        Data.Add(Tile.Tree,new Tree());
    }
    
    public Dictionary<Tile, IInteractable> Data = new Dictionary<Tile, IInteractable>();
}
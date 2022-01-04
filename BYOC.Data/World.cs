namespace BYOC.Data;

public class World
{
    public World(int width, int height)
    {
        Width = width;
        Height = height;
        Tiles = new Tile[Width, Height];
        Seed();
    }
    
    public int Width { get; set; }
    public int Height { get; set; }
    public Tile[,] Tiles { get; set; }
    
    public void Seed()
    {
        Tiles = new Tile[Width, Height];

        Array tileTypes = Enum.GetValues(typeof(Tile));
        Random random = new Random();
        
        for (int i = 0; i < Width; i++)
        {
            for (int j = 0; j < Height; j++)
            {
                Tiles[i, j] = (Tile)(tileTypes.GetValue(random.Next(tileTypes.Length)) ?? Tile.Dirt);;
            }
        }
    }
}
namespace BYOC.Data.Objects;
public record Position
{
    public Position()
    {
        
    }
    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
    public int X { get; set; }
    public int Y { get; set; }
}
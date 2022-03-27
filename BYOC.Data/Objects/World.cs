using System.Diagnostics.CodeAnalysis;

namespace BYOC.Data.Objects;
public class World : IWorld
{
    public int Width { get; set; }
    public int Height { get; set; }
    [AllowNull]
    public List<Node> Nodes { get; set; }

    public BasicList<Player> Players { get; set; } = new();

    public void Reset(int width, int height)
    {
        Width = width;
        Height = height;
        Nodes = new List<Node>();
    }
}
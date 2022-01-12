namespace BYOC.Data.Objects;

public interface IWorld
{
    int Width { get; set; }
    int Height { get; set; }
    Node?[,] Nodes { get; set; }
    BasicList<Player> Players { get; set; }
    void Reset(int width, int height);
}
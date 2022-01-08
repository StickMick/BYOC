namespace BYOC.Data.Services;

public interface ITileService
{
    Node? GetTile(int x, int y);
    Node? GetTile(Position position);
    void Seed(int width, int height);
    IEnumerable<Position> GetPath(Position from, Position to);
}
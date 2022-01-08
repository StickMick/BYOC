namespace BYOC.Data.Repositories;

public interface ITileRepository
{
    Node? GetTile(int x, int y);
    Node? GetTile(Position position);
    void Seed(int width, int height);
    IEnumerable<Node?> GetWalkableAdjacentSquares(Position position);
}
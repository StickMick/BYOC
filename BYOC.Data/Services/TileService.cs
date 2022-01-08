using BYOC.Data.Repositories;

namespace BYOC.Data.Services;

public class TileService : ITileService
{
    private readonly ITileRepository _tileRepository;

    public TileService(ITileRepository tileRepository)
    {
        _tileRepository = tileRepository;
    }

    public Node? GetTile(int x, int y)
    {
        return _tileRepository.GetTile(x, y);
    }

    public Node? GetTile(Position position)
    {
        return _tileRepository.GetTile(position);
    }

    public void Seed(int width, int height)
    {
        _tileRepository.Seed(width, height);
    }
    
    public IEnumerable<Position> GetPath(Position from, Position to)
    {
        Node? current = null;
        Node? start = _tileRepository.GetTile(from);
        Node? target = _tileRepository.GetTile(to);

        if (start == null || target == null)
            throw new Exception("Invalid start or end location");
        
        var openList = new List<Node?>();
        var closedList = new List<Node?>();
        int g = 0;
        
        openList.Add(start);

        while (openList.Count > 0)
        {
            var lowest = openList.Min(l => l.F);
            current = openList.First(l => Math.Abs(l.F - lowest) < 0.01);
            closedList.Add(current);
            openList.Remove(current);
            if (closedList.FirstOrDefault(l =>
                    l.Position.X == target.Position.X && l.Position.Y == target.Position.Y) != null)
                break;
            var adjacentSquares = _tileRepository.GetWalkableAdjacentSquares(current.Position);
            g++;

            foreach (var adjacentSquare in adjacentSquares)
            {
                // if this adjacent square is already in the closed list, ignore it
                if (closedList.FirstOrDefault(l => l.Position.X == adjacentSquare.Position.X
                                                   && l.Position.Y == adjacentSquare.Position.Y) != null)
                    continue;

                // if it's not in the open list...
                if (openList.FirstOrDefault(l => l.Position.X == adjacentSquare.Position.X
                                                 && l.Position.Y == adjacentSquare.Position.Y) == null)
                {
                    // compute its score, set the parent
                    adjacentSquare.G = g;
                    adjacentSquare.H = ComputeHScore(adjacentSquare.Position.X,
                        adjacentSquare.Position.Y, target.Position.X, target.Position.Y);
                    adjacentSquare.ParentNode = current;

                    // and add it to the open list
                    openList.Insert(0, adjacentSquare);
                }
                else
                {
                    // test if using the current G score makes the adjacent square's F score
                    // lower, if yes update the parent because it means it's a better path
                    if (g + adjacentSquare.H < adjacentSquare.F)
                    {
                        adjacentSquare.G = g;
                        adjacentSquare.ParentNode = current;
                    }
                }
            }
        }

        List<Position> path = new List<Position>();
        while (current != null)
        {
            path.Add(current.Position);
            current = current.ParentNode;
        }



        return path;
    }
    
    static int ComputeHScore(int fromX, int fromY, int toX, int toY)
    {
        return Math.Abs(toX - fromX) + Math.Abs(toY - fromY);
    }
}
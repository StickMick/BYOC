using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public class UnitRepository
{
    public UnitRepository()
    {
        
    }

    public List<Unit> Units { get; set; } = new List<Unit>();

    public Unit? GetUnit(int x, int y) => Units.FirstOrDefault(u => u.Position.X == x && u.Position.Y == y);
}
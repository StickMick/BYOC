using BYOC.Data.Objects;

namespace BYOC.Data.Repositories;

public class UnitRepository : IUnitRepository
{
    public UnitRepository()
    {
        
    }

    public BasicList<Unit> Units { get; set; } = new ();

    public Unit? GetUnit(int x, int y) => Units.FirstOrDefault(u => u!.Position!.X == x && u.Position.Y == y);
}
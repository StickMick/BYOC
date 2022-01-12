using BYOC.Data.Objects;

namespace BYOC.Data.Services;

public interface IUnitService
{
    Unit? GetUnit(Guid Id);
}
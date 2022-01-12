using BYOC.Data.Objects;

namespace BYOC.Data.Services;

/// <summary>
/// For getting information about the particular user session
/// </summary>
public interface ISessionService
{
    Guid GetPlayerId();
    Player GetPlayer();
}
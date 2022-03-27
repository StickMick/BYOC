using AutoMapper;
using BYOC.Data.Objects;
using BYOC.Shared.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ILogger = Serilog.ILogger;

namespace BYOC.Server.Hubs;

public class GameHub : Hub
{
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IWorld _world;

    public GameHub(
        IMapper mapper,
        ILogger logger,
        IWorld world)
    {
        _mapper = mapper;
        _logger = logger;
        _world = world;
    }
    
    [Authorize(AuthenticationSchemes = "Bearer")]
    public async Task GetWorldAsync()
    {
        var dto = _mapper.Map<WorldDTO>(_world);
        await Clients.All.SendAsync("GetWorldAsync", dto);
    }
}
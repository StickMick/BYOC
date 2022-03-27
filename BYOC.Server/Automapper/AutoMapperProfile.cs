using AutoMapper;
using BYOC.Data.Objects;
using BYOC.Shared.DTOs;

namespace BYOC.Server.Automapper;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<World, WorldDTO>();
        CreateMap<Node, NodeDTO>();
        CreateMap<Position, PositionDTO>();
    }
}
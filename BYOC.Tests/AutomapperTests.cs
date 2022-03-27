using AutoMapper;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;
using BYOC.Server.Automapper;
using BYOC.Shared.DTOs;
using NUnit.Framework;

namespace BYOC.Tests;

public class AutomapperTests
{
    private IMapper _mapper;
    
    [SetUp]
    public void Setup()
    {
        _mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<AutoMapperProfile>();
        }).CreateMapper();
    }

    [Test]
    public void CanMapWorld()
    {
        _mapper.Map<WorldDTO>(new World());
        Assert.Pass();
    }
}
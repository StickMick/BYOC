using System.Text;
using AutoMapper;
using BYOC.Data.Controllers;
using BYOC.Data.Events;
using BYOC.Data.Helpers;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore;
using BYOC.Server.Areas.Identity;
using BYOC.Server.Automapper;
using BYOC.Server.Data;
using BYOC.Server.Data.Entities;
using BYOC.Server.Helpers;
using BYOC.Server.Hubs;
using BYOC.Server.Middleware;
using BYOC.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using ILogger = Serilog.ILogger;


var builder = WebApplication.CreateBuilder(args);

ConfigurationManager configuration = builder.Configuration;

var logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console()
    .CreateLogger();

builder.Logging.AddSerilog(logger);
builder.Services.AddSingleton<ILogger>(logger);
builder.Services.AddAutoMapper(mapper => mapper.AddProfile<AutoMapperProfile>());

SetupDatabases(builder);

SetupSecurity(builder);

SetupWeb(builder);

SetupGame(builder);

builder.Services.AddScoped<DatabaseSeeder>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapBlazorHub();

app.UseMiddleware<JwtMiddleware>();

app.MapHub<GameHub>("/game");

app.MapFallbackToPage("/_Host");

await SeedDatabase();

StartGame(app);

app.Run();

void SetupSecurity(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services
        .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<ApplicationUser>>();

    webApplicationBuilder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.RequireHttpsMetadata = false;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidAudience = configuration["JWT:ValidAudience"],
                ValidIssuer = configuration["JWT:ValidIssuer"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]))
            };
        });
}

void SetupDatabases(WebApplicationBuilder webApplicationBuilder)
{
    var connectionString = webApplicationBuilder.Configuration.GetConnectionString("DefaultConnection");
    webApplicationBuilder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlite(connectionString));
    webApplicationBuilder.Services.AddDatabaseDeveloperPageExceptionFilter();
    webApplicationBuilder.Services
        .AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
        .AddEntityFrameworkStores<ApplicationDbContext>();
}

void SetupWeb(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddRazorPages();
    webApplicationBuilder.Services.AddServerSideBlazor();
    webApplicationBuilder.Services.AddResponseCompression(opts =>
    {
        opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
            new[] {"application/octet-stream"});
    });
    webApplicationBuilder.Services.AddSwaggerGen();
}

void SetupGame(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services.AddSingleton<IWorld, World>(e => new World());

    // Raw data - singleton for now since they are stored in memory on the object
    webApplicationBuilder.Services.AddSingleton<ITileRepository, TileRepository>();
    webApplicationBuilder.Services.AddSingleton<IUnitRepository, UnitRepository>(provider =>
    {
        var world = provider.GetRequiredService<IWorld>();
        var logger = provider.GetRequiredService<ILogger>();
        var unitRepository = new UnitRepository(world, logger);
        
        // Bind events from the game engine to methods in the API
        unitRepository.OnUnitCreated += (_, unitCreatedEventArgs) =>
        {
            unitCreatedEventArgs.Unit.OnMove += (_, unitMovedEventArgs) =>
            {
                var gameHub = provider.GetRequiredService<IHubContext<GameHub>>();
                var mapper = provider.GetRequiredService<IMapper>();
                var dto = mapper.Map<UnitMovedEventArgs>(unitMovedEventArgs);
                gameHub.Clients.All.SendAsync("UnitMoved", dto);
            };
        };

        return unitRepository;
    });

    // Data access and logic
    webApplicationBuilder.Services.AddTransient<IWorldService, WorldService>();
    webApplicationBuilder.Services.AddTransient<ITileService, TileService>();
    webApplicationBuilder.Services.AddTransient<IUnitService, UnitService>();

    // commands - typically player specific
    webApplicationBuilder.Services.AddTransient<IUnitController, UnitController>();
    webApplicationBuilder.Services.AddTransient<IGameController, GameController>();

    // current user information
    webApplicationBuilder.Services.AddTransient<ISessionService, SessionService>();

    // ~~~~~~~~~~~~~~
}

void StartGame(WebApplication webApplication)
{
    var worldService = webApplication.Services.GetRequiredService<IWorldService>();
    var tileService = webApplication.Services.GetRequiredService<ITileService>();
    var gameController = webApplication.Services.GetRequiredService<IGameController>();
    var unitRepository = webApplication.Services.GetRequiredService<IUnitRepository>();
    var unitController = webApplication.Services.GetRequiredService<IUnitController>();
    
    // create the world
    var width = Convert.ToInt32(configuration["Game:Width"]);
    var height = Convert.ToInt32(configuration["Game:Height"]);
    
    tileService.Seed(width,height);

    var testPlayer = worldService.AddPlayer(new Player());
    var unit = unitRepository.AddUnit(testPlayer.Id, 1, 1)!;
    
    gameController.Start(options =>
    {
        options.TickRate = 1_000;
    });
    
    unitController.TryMoveUnit(unit.Id, width-1, height-1);
}

async Task SeedDatabase() //can be placed at the very bottom under app.Run()
{
    using (var scope = app.Services.CreateScope())
    {
        var dbInitializer = scope.ServiceProvider.GetRequiredService<DatabaseSeeder>();
        await dbInitializer.MigrateAsync();
        await dbInitializer.SeedAsync();
    }
}
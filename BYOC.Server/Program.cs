using System.Text;
using BYOC.Data.Controllers;
using BYOC.Data.Helpers;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BYOC.Server.Areas.Identity;
using BYOC.Server.Automapper;
using BYOC.Server.Data;
using BYOC.Server.Hubs;
using BYOC.Server.Middleware;
using BYOC.Server.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.ResponseCompression;
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

StartGame(app);

app.Run();

void SetupSecurity(WebApplicationBuilder webApplicationBuilder)
{
    webApplicationBuilder.Services
        .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

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
        .AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
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

    // Raw data
    webApplicationBuilder.Services.AddSingleton<ITileRepository, TileRepository>();
    webApplicationBuilder.Services.AddSingleton<IUnitRepository, UnitRepository>();

    // Data access and logic
    webApplicationBuilder.Services.AddSingleton<IWorldService, WorldService>();
    webApplicationBuilder.Services.AddSingleton<ITileService, TileService>();
    webApplicationBuilder.Services.AddSingleton<IUnitService, UnitService>();

    // commands - typically player specific
    webApplicationBuilder.Services.AddSingleton<IUnitController, UnitController>();
    webApplicationBuilder.Services.AddSingleton<IGameController, GameController>();

    // current user information, singleton for testing until hooked into the lifecycle
    webApplicationBuilder.Services.AddSingleton<ISessionService, SessionService>();

    // ~~~~~~~~~~~~~~
}

void StartGame(WebApplication webApplication)
{
    var world = webApplication.Services.GetRequiredService<IWorld>();
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
    
    MapVisualizer.DrawToConsole(world);
    
    gameController.Start(options =>
    {
        options.TickRate = 1_000;
        // options.Tick += (sender, eventArgs) =>
        // {
        //     MapVisualizer.DrawToConsole(world);
        // };
    });
    
    unitController.TryMoveUnit(unit.Id, width-1, height-1);
}
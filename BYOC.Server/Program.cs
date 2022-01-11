using BYOC.Data;
using BYOC.Data.Controllers;
using BYOC.Data.Helpers;
using BYOC.Data.Objects;
using BYOC.Data.Repositories;
using BYOC.Data.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using BYOC.Server.Areas.Identity;
using BYOC.Server.Data;
using BYOC.Server.Hubs;
using Microsoft.AspNetCore.ResponseCompression;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();
builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services
    .AddScoped<AuthenticationStateProvider, RevalidatingIdentityAuthenticationStateProvider<IdentityUser>>();

// Placeholder for adding game stuff

builder.Services.AddSingleton<World>(e => new World());
builder.Services.AddSingleton<ITileRepository, TileRepository>();
builder.Services.AddSingleton<IUnitRepository, UnitRepository>();
builder.Services.AddSingleton<IWorldService, WorldService>();
builder.Services.AddSingleton<ITileService, TileService>();
builder.Services.AddSingleton<IUnitController, UnitController>();
builder.Services.AddSingleton<IGameController, GameController>();

builder.Services.AddSingleton<CancellationTokenSource>();

// ~~~~~~~~~~~~~~

builder.Services.AddResponseCompression(opts =>
{
    opts.MimeTypes = ResponseCompressionDefaults.MimeTypes.Concat(
        new[] { "application/octet-stream" });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
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

app.MapHub<DemoHub>("/demo");

app.MapFallbackToPage("/_Host");

app.Services.GetService<ITileService>()?.Seed(30,30);
app.Services.GetService<IUnitRepository>()?.Units.Add(new Unit(5,5));
MapVisualizer.DrawToConsole(app.Services.GetService<World>()!);
app.Services.GetService<IGameController>()?.Start();

app.Run();
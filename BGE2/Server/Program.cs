using BGE2.Server.Data.Boardgames;
using BGE2.Server.Data.Interfaces;
using BGE2.Server.Data.MongoDB;
using BGE2.Server.Data.SQLite;
using BGE2.Server.Extensions;
using Microsoft.EntityFrameworkCore;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.AddDbContext<SqliteDBContext>(options =>
{
    options.UseSqlite("Filename=users.db");
});

builder.Services.AddScoped<IUserRightsRepository, SqliteRepository>();

builder.Services.AddScoped<IEventRepository, MongoDBRepository>(_ =>
    new(builder.Configuration
        .GetConnectionString("MongoDB"),
        "Events",
        "BGE-DB"
    ));

builder.Services.AddScoped<BoardgamesRepository>();

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment()) app.UseWebAssemblyDebugging();
else app.UseHsts();

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.MapRazorPages();

//-- Events
app.MapEventEndpoints();

//-- Games
app.MapGamesEndpoints();

//-- User Rights
app.MapUserRightsEndpoints();

app.MapFallbackToFile("index.html");
app.Run();

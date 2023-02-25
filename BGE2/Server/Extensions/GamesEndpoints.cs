using BGE2.Server.Data.Boardgames;

namespace BGE2.Server.Extensions;

public static class GamesEndpoints
{
    public static IApplicationBuilder MapGamesEndpoints(this WebApplication app)
    {
        app.MapGet("/Game/All", async (BoardgamesRepository games) => await games.GetAllAsync());

        return app;
    }
}

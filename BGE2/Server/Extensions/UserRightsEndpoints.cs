using BGE2.Server.Data.Interfaces;
using BGE2.Shared;

namespace BGE2.Server.Extensions;

public static class UserRightsEndpoints
{
    public static IApplicationBuilder MapUserRightsEndpoints(this WebApplication app)
    {
        app.MapGet("/user/{email}", async (IUserRightsRepository repo, string email) => await repo.GetAsync(email));

        app.MapGet("/user/all", async (IUserRightsRepository repo) => await repo.GetAllAsync());
        
        app.MapPost("/user", async (IUserRightsRepository repo, UserDto user) => await  repo.AddAsync(user));

        app.MapDelete("/user/{email}", async (IUserRightsRepository repo, string email) => await repo.DeleteAsync(email));

        return app;
    }
}

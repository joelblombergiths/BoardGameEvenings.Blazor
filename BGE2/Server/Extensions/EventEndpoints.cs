using BGE2.Server.Data.Interfaces;
using BGE2.Shared;

namespace BGE2.Server.Extensions;

public static class EventEndpoints
{
    public static IApplicationBuilder MapEventEndpoints(this WebApplication app)
    {
        app.MapGet("/Event/All", async (IEventRepository repo) => await repo.GetAllAsync());

        app.MapGet("/Event/{id}", async (IEventRepository repo, string id) => await repo.GetAsync(id));

        app.MapPost("/Event", async (IEventRepository repo, EventDto addEvent) => await repo.AddAsync(addEvent));

        app.MapPut("/Event/{id}", async (IEventRepository repo, string id, EventDto updateEvent) => await repo.UpdateAsync(id, updateEvent));

        app.MapDelete("/Event/{id}", async (IEventRepository repo, string id) => await repo.DeleteAsync(id));

        app.MapPost("/Event/{id}/Attendee", async (IEventRepository repo, string id, AttendeeDto attendee) => await repo.AddAttendeeAsync(id, attendee));

        app.MapDelete("/Event/{eventId}/Attendee/{attendeeId}", (IEventRepository repo, string eventId, int attendeeId) => repo.RemoveAttendeeAsync(eventId, attendeeId));
        
        return app;
    }
}

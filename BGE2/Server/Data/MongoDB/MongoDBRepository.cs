using BGE2.Server.Data.Interfaces;
using BGE2.Server.Data.Models;
using BGE2.Shared;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BGE2.Server.Data.MongoDB;

public class MongoDBRepository : IEventRepository
{
    private readonly MongoDBContext<EventModel> _context;
    private readonly FilterDefinitionBuilder<EventModel> _filter;

    public MongoDBRepository(string? connectionString, string collectionName, string databaseName)
    {
        if (string.IsNullOrEmpty(connectionString)) throw new("Connectionstring not found");
        _context = new(connectionString, collectionName, databaseName);
        _filter = Builders<EventModel>.Filter;
    }

    private static string GetTopVote(List<AttendeeModel>? attendees)
    {
        if (attendees == null) return "Any";
        if (attendees.Count == 0 ) return "Any";
        return attendees
            .GroupBy(a => a.Vote)
            .Select(a => new { game = a.Key, count = a.Count() })
            .OrderByDescending(a => a.count)
            .First()
            .game;
    }

    public async Task<List<EventDto>> GetAllAsync()
    {
        List<EventModel> list = await _context.Collection
            .Find(_filter.Empty)
            .ToListAsync();

        return list.Select(e => new EventDto
        {
            Id = e.Id.ToString(),
            Name = e.Name,
            Date = e.Date,
            TopVote = GetTopVote(e.Attendees)
        }).ToList();
    }

    public async Task<EventDto> GetAsync(string id)
    {
        EventModel e = await _context.Collection
            .Find(_filter.Eq("_id", new ObjectId(id)))
            .FirstOrDefaultAsync();

        List<AttendeeDto> attendees = new();
        if (e.Attendees?.Count > 0)
        {
            attendees.AddRange(e.Attendees.Select(a => new AttendeeDto
            {
                Id = a.Id,
                Name = a.Name,
                Vote = a.Vote
            }));
        }

        return new()
        {
            Id = e.Id.ToString(),
            Name = e.Name,
            Date = e.Date,
            Attendees = attendees,
            TopVote = GetTopVote(e.Attendees)
        };
    }

    public async Task<IResult> AddAsync(EventDto e)
    {
        try
        {
            await _context.Collection.InsertOneAsync(new()
            {
                Name = e.Name,
                Date = e.Date
            });

            return Results.Ok();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public async Task<IResult> UpdateAsync(string id, EventDto e)
    {
        try
        {
            FilterDefinition<EventModel> filter = _filter.Eq("_id", new ObjectId(id));
            if (await _context.Collection.Find(filter).FirstOrDefaultAsync() == null) return Results.NotFound();

            UpdateResult result = await _context.Collection
                .UpdateOneAsync(filter, Builders<EventModel>.Update
                    .Set(f => f.Name, e.Name)
                    .Set(f => f.Date, e.Date));
            return result.IsAcknowledged && result.ModifiedCount > 0 ? Results.Ok() : Results.BadRequest();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public async Task<IResult> DeleteAsync(string id)
    {
        try
        {
            FilterDefinition<EventModel> filter = _filter.Eq("_id", new ObjectId(id));
            if (await _context.Collection.Find(filter).FirstOrDefaultAsync() == null) Results.NotFound();

            DeleteResult result = await _context.Collection.DeleteOneAsync(filter);
            return result.IsAcknowledged && result.DeletedCount > 0 ? Results.Ok() : Results.BadRequest();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public async Task<IResult> AddAttendeeAsync(string id, AttendeeDto attendee)
    {
        try
        {
            FilterDefinition<EventModel> filter = _filter.Eq("_id", new ObjectId(id));
            EventModel e = await _context.Collection.Find(filter).FirstOrDefaultAsync();
            if (e == null) return Results.NotFound();

            UpdateResult result = await _context.Collection
                .UpdateOneAsync(filter, Builders<EventModel>.Update
                    .Set(f => f.NextAttendeeId, e.NextAttendeeId + 1)
                    .Push("Attendees", new AttendeeModel
                    {
                        Id = e.NextAttendeeId,
                        Name = attendee.Name,
                        Vote = attendee.Vote
                    }));

            return result.IsAcknowledged && result.ModifiedCount > 0 ? Results.Ok() : Results.BadRequest();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }

    public async Task<IResult> RemoveAttendeeAsync(string eventId, int attendeeId)
    {
        try
        {
            FilterDefinition<EventModel> filter = _filter.Eq("_id", new ObjectId(eventId));
            if (await _context.Collection.Find(filter).FirstOrDefaultAsync() == null) return Results.NotFound();

            UpdateResult result = await _context.Collection
                .UpdateOneAsync(filter,
                    Builders<EventModel>.Update
                        .PullFilter(e => e.Attendees, a => a.Id == attendeeId)
                );

            return result.IsAcknowledged && result.ModifiedCount > 0 ? Results.Ok() : Results.BadRequest();
        }
        catch (Exception ex)
        {
            return Results.BadRequest(ex.Message);
        }
    }
}

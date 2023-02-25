using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace BGE2.Server.Data.Models;

public class EventModel
{
    [BsonId]
    public ObjectId Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public int NextAttendeeId { get; set; } = 0;
    public List<AttendeeModel>? Attendees { get; set; } = new();
}
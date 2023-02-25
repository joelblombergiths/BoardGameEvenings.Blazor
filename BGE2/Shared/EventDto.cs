namespace BGE2.Shared;

public class EventDto
{
    public string Id { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string Date { get; init; } = string.Empty;
    public List<AttendeeDto> Attendees { get; init; } = new();
    public string TopVote { get; init; } = "Any";
}

using BGE2.Shared;

namespace BGE2.Server.Data.Interfaces;

public interface IEventRepository
{
    Task<List<EventDto>> GetAllAsync();
    Task<EventDto> GetAsync(string id);
    Task<IResult> AddAsync(EventDto item);
    Task<IResult> UpdateAsync(string id, EventDto item);
    Task<IResult> DeleteAsync(string id);
    Task<IResult> AddAttendeeAsync(string id, AttendeeDto attendee);
    Task<IResult> RemoveAttendeeAsync(string eventId, int attendeeId);
}
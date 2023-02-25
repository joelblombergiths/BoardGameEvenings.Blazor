using BGE2.Shared;

namespace BGE2.Server.Data.Interfaces;

public interface IUserRightsRepository
{
    Task<List<UserDto>> GetAllAsync();
    Task<UserDto> GetAsync(string email);
    Task<IResult> AddAsync(UserDto user);
    Task<IResult> DeleteAsync(string email);
}
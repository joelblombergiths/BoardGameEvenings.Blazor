using BGE2.Server.Data.Interfaces;
using BGE2.Server.Data.Models;
using BGE2.Shared;
using Microsoft.EntityFrameworkCore;

namespace BGE2.Server.Data.SQLite;

public class SqliteRepository : IUserRightsRepository
{
    private readonly SqliteDBContext _context;

    public SqliteRepository(SqliteDBContext context)
    {
        _context = context;
    }

    public async Task<List<UserDto>> GetAllAsync()
    {
        List<UserModel> res = await _context.UserRights.ToListAsync();
        return res.Select(u => new UserDto
        {
            Email = u.Email,
            Role = u.Role
        }).ToList();
    }

    public async Task<UserDto> GetAsync(string email)
    {
        UserModel? findUser = await _context.UserRights.FindAsync(email);
        
        return new()
        {
            Email = findUser?.Email ?? string.Empty,
            Role = findUser?.Role ?? "{NOTFOUND}"
        };
    }

    public async Task<IResult> AddAsync(UserDto user)
    {
        await _context.AddAsync(new UserModel
        {
            Email = user.Email,
            Role = user.Role
        });

        await _context.SaveChangesAsync();
        return Results.Ok();
    }

    public async Task<IResult> DeleteAsync(string email)
    {
        _context.UserRights.Remove(new() { Email = email });
        await _context.SaveChangesAsync();
        return Results.Ok();
    }
}

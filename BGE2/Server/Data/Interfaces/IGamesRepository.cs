namespace BGE2.Server.Data.Interfaces;

public interface IGamesRepository
{
    Task<List<string>> GetAllAsync();
}
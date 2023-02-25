using BGE2.Server.Data.Interfaces;

namespace BGE2.Server.Data.Boardgames;

public class BoardgamesRepository : IGamesRepository
{
    private readonly HttpClient _client;

    public BoardgamesRepository()
    {
        _client = new()
        {
            BaseAddress = new("https://raw.githubusercontent.com/joelblombergiths/boardgames/main/README.md")
        };
    }

    public async Task<List<string>> GetAllAsync()
    {
        string res = await _client.GetStringAsync("");

        if (string.IsNullOrEmpty(res)) return new();

        return res
            .Split("\n", StringSplitOptions.TrimEntries | StringSplitOptions.TrimEntries)
            .Skip(2)
            .SkipLast(1)
            .Select(g => g.TrimStart('-').TrimStart())
            .ToList();
    }
}

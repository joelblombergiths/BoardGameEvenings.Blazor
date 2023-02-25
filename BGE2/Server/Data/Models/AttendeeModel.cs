namespace BGE2.Server.Data.Models;

public class AttendeeModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public string Vote { get; init; } = "Any";
}
using System.ComponentModel.DataAnnotations;

namespace BGE2.Shared;

public class AttendeeDto
{
    public int Id { get; init; }

    [Required(ErrorMessage = "Please enter a name")]
    public string Name { get; set; } = string.Empty;
    public string Vote { get; set; } = "Any";
}

using System.ComponentModel.DataAnnotations;

namespace BGE2.Shared;

public class EditEventDto
{
    [Required(ErrorMessage = "Please enter a name for the event.")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Please enter a date for the event.")]
    public DateTime Date { get; set; } = DateTime.Now;
}

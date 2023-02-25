using System.ComponentModel.DataAnnotations;

namespace BGE2.Server.Data.Models;

public class UserModel
{
    [Key]
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
}
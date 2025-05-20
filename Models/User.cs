using System.ComponentModel.DataAnnotations;
namespace perAPI.Models;
public enum Role { Admin = 0, Coach = 1, User = 2 }
public class User
{
    [Key]
    public int Id { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; } = null!;

    [Required, MinLength(6)]
    public byte[] PasswordHash { get; set; } = null!;

    [Required]
    public byte[] PasswordSalt { get; set; } = null!;

    [Required]
    public Role Role { get; set; }

    public int? CoachId { get; set; }
}
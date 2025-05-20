using System.ComponentModel.DataAnnotations;
namespace perAPI.Dto.User;

public class UserRegisterDto
{
    public UserRegisterDto(string password)
    {
        Password = password;
    }

    [Required, EmailAddress]
    public string Email { get; set; } = null!;


    [Required, MinLength(6)]
    public string Password { get; set; }

    [Required]
    public string? Role { get; set; } // "User", "Coach", "Admin"
}
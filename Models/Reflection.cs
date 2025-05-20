using System.ComponentModel.DataAnnotations;
namespace perAPI.Models;

public class Reflection
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; } = null!;

    public DateTime Date { get; set; } = DateTime.UtcNow;

    [Required]
    public int HabitId { get; set; }
    
    public Habit Habit { get; set; } = null!;

    [Required]
    public int UserId { get; set; }
};
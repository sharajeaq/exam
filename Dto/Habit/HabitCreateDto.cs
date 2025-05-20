using System.ComponentModel.DataAnnotations;
namespace perAPI.Dto.Habit;

public class HabitCreateDto
{
    [Required]
    public string Name { get; set; } = null!;
}
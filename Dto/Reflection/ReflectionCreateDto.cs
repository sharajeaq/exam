using System.ComponentModel.DataAnnotations;
namespace perAPI.Data.Reflection;

public class ReflectionCreateDto
{
    private string _content = null!;
    private int _habitId;

    [Required]
    public string Content
    {
        get => _content;
        set => _content = value;
    }

    [Required]
    public int HabitId
    {
        get => _habitId;
        set => _habitId = value;
    }
};
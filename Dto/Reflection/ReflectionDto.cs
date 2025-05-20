namespace perAPI.Dto.Reflection;

public class ReflectionDto
{
    private string _content = null!;
    private DateTime _date;
    private int _habitId;
    private string _habitName = null!;

    public string Content
    {
        get => _content;
        set => _content = value;
    }

    public DateTime Date
    {
        get => _date;
        set => _date = value;
    }

    public int HabitId
    {
        get => _habitId;
        set => _habitId = value;
    }

    public string HabitName
    {
        get => _habitName;
        set => _habitName = value;
    }
}
namespace schedule_bot.Entities;

public class User
{
    public long Id { get; set; }
    public bool IsAdmin { get; set; }
    public UserSettings Settings { get; set; } = default!;
    public List<UserTask> Tasks { get; set; } = [];
    public LastMenu LastMenu { get; set; } = default!;
}
public class LastMenu
{
    public string Name { get; set; } = default!;
    public Dictionary<string, string> Data { get; set; } = [];
}
public class UserSettings
{
    public bool DeadlineEnabled { get; set; }
    public bool BeginningEnabled { get; set; }
}
public class UserTask
{
    public string Description { get; set; } = default!;
    public DateTime DueDate { get; set; }
}

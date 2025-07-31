namespace schedule_bot.Entities;

public class Schedule
{
    public Guid Id { get; set; }
    public List<ScheduleItem> Items { get; set; } = [];
    public int LastParity { get; set; }
    public DateTime UploadDate { get; set; }
}
public class ScheduleItem
{
    public DayOfWeek DayOfWeek { get; set; }
    public string SubjectName { get; set; } = default!;
    public string LecturerName { get; set; } = default!;
    public string Location { get; set; } = default!;
    public TimeSpan Time { get; set; }
    public int Parity { get; set; }
}

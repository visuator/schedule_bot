using LiteDB;
using schedule_bot.Entities;

namespace schedule_bot.Services;

public interface IScheduleRepository
{
    string[] GetSubjectNames();
    void Add(Schedule schedule);
}
public class ScheduleRepository(LiteDatabase db) : IScheduleRepository
{
    private ILiteCollection<Schedule> _scheduleCollection = db.GetCollection<Schedule>();

    public string[] GetSubjectNames()
    {
        var lastSchedule = _scheduleCollection.Query()
            .OrderByDescending(x => x.UploadDate)
            .First();
        var subjects = lastSchedule.Items
            .Select(x => x.SubjectName)
            .Distinct()
            .ToArray();
        return subjects;
    }

    public void Add(Schedule schedule)
    {
        _scheduleCollection.Insert(schedule);
    }
}
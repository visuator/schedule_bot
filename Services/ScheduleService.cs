using LiteDB;
using schedule_bot.Entities;

namespace schedule_bot.Services;

public interface IScheduleRepository
{
    void Add(Schedule schedule);
}
public class ScheduleRepository(LiteDatabase database) : IScheduleRepository
{
    public void Add(Schedule schedule)
    {
        database.GetCollection<Schedule>().Insert(schedule);
    }
}

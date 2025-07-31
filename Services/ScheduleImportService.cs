using System.Text;
using ExcelDataReader;
using schedule_bot.Entities;

namespace schedule_bot.Services;

public interface IImportScheduleService
{
    void Import(Stream stream, DateTime uploadDate);
}
public class ExcelScheduleImportService(IScheduleRepository scheduleRepository) : IImportScheduleService
{
    private static readonly Dictionary<string, DayOfWeek> _dayOfWeekMap = new()
    {
        ["Понедельник"] = DayOfWeek.Monday,
        ["Вторник"] = DayOfWeek.Tuesday,
        ["Среда"] = DayOfWeek.Wednesday,
        ["Четверг"] = DayOfWeek.Thursday,
        ["Пятница"] = DayOfWeek.Friday,
        ["Суббота"] = DayOfWeek.Saturday,
        ["Воскресенье"] = DayOfWeek.Sunday
    };

    public void Import(Stream stream, DateTime uploadDate)
    {
        using var reader = ExcelReaderFactory.CreateReader(stream, new() { FallbackEncoding = Encoding.UTF8 });
        var dataset = reader.AsDataSet();
        var workingTable = dataset.Tables[0];
        var schedule = new Schedule() { UploadDate = uploadDate };
        for (var i = 1; i < workingTable.Rows.Count; i++)
        {
            var row = workingTable.Rows[i];
            schedule.Items.Add(new()
            {
                Parity = int.Parse(row[1].ToString() ?? ""),
                DayOfWeek = _dayOfWeekMap[row[2].ToString() ?? ""],
                SubjectName = row[3].ToString() ?? "",
                LecturerName = row[4].ToString() ?? "",
                Location = row[5].ToString() ?? "",
                Time = TimeSpan.Parse(row[6].ToString() ?? "")
            });
        }
        scheduleRepository.Add(schedule);
    }
}

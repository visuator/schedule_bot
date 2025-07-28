using ExcelDataReader;
using schedule_bot.Entities;

namespace schedule_bot.Services;

public interface IImportScheduleService
{
    void Import(Stream stream, DateTime uploadDate);
}
public class ExcelScheduleImportService(IScheduleRepository scheduleRepository) : IImportScheduleService
{
    public void Import(Stream stream, DateTime uploadDate)
    {
        using var reader = ExcelReaderFactory.CreateReader(stream);
        var dataset = reader.AsDataSet();
        var workingTable = dataset.Tables[0];
        var schedule = new Schedule() { UploadDate = uploadDate };
        for (var i = 1; i < workingTable.Rows.Count; i++)
        {
            var row = workingTable.Rows[i];
            schedule.Items.Add(new()
            {
                Parity = int.Parse(row[1].ToString() ?? ""),
                SubjectName = row[2].ToString() ?? "",
                LecturerName = row[3].ToString() ?? "",
                Location = row[4].ToString() ?? "",
                Time = TimeSpan.Parse(row[5].ToString() ?? "")
            });
        }
        scheduleRepository.Add(schedule);
    }
}

namespace schedule_bot.Services;

public record AddVacation(DateTime Start, DateTime End);
public interface IVacationService
{
    void AddVacation(AddVacation dto);
}
public class VacationService : IVacationService
{
    public void AddVacation(AddVacation dto)
    {

    }
}

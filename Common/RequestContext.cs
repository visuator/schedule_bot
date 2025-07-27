using schedule_bot.Entities;

namespace schedule_bot.Commands;

public class RequestContext
{
    public User User { get; set; } = default!;
    public Telegram.Bot.Types.Message? Message { get; set; }
    public Telegram.Bot.Types.CallbackQuery? CallbackQuery { get; set; }
    public string? CurrentState { get; set; }
}
public class VacationState
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
using Telegram.Bot.Types;
using User = schedule_bot.Entities.User;

namespace schedule_bot.Commands;

public class RequestContext
{
    public User User { get; set; } = default!;
    public Message? Message { get; set; }
    public CallbackQuery? CallbackQuery { get; set; }
}

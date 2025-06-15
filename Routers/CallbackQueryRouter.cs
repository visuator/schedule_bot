using Telegram.Bot.Types;

namespace schedule_bot.Routers;

public interface ICallbackQueryRouter
{
    Task HandleCallbackQuery(CallbackQuery callbackQuery);
}
public class CallbackQueryRouter : ICallbackQueryRouter
{
    public Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        return Task.CompletedTask;
    }
}

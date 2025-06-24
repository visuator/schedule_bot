using schedule_bot.Commands;
using schedule_bot.Extensions;
using schedule_bot.Menus;
using schedule_bot.Services;
using Telegram.Bot.Types;

namespace schedule_bot.Routers;

public interface ICallbackQueryRouter
{
    Task HandleCallbackQuery(CallbackQuery callbackQuery);
}
public class CallbackQueryRouter(
    IUserRepository userRepository,
    MenuService service
    ) : ICallbackQueryRouter
{
    public Task HandleCallbackQuery(CallbackQuery callbackQuery)
    {
        ArgumentNullException.ThrowIfNull(callbackQuery.Message);
        var user = userRepository.GetOrCreateDefault(new(callbackQuery.Message.GetUserId(), false));
        var context = new RequestContext()
        {
            User = user,
            Message = null,
            CallbackQuery = callbackQuery
        };
        return service.Route(context);
    }
}

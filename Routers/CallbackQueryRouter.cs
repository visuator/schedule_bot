using Microsoft.Extensions.Caching.Memory;
using schedule_bot.Commands;
using schedule_bot.Extensions;
using schedule_bot.Menus.Impl;
using schedule_bot.Services;
using Telegram.Bot.Types;

namespace schedule_bot.Routers;

public interface ICallbackQueryRouter
{
    Task HandleCallbackQuery(CallbackQuery callbackQuery);
}
public class CallbackQueryRouter(
    IUserRepository userRepository,
    MediatRMenuRouter menuRouter,
    MenuStorage storage,
    IMemoryCache cache
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
            CallbackQuery = callbackQuery,
            CurrentState = cache.Get<string>($"{user.Id}-current_state"),
        };
        return menuRouter.Route(storage.GetAll(user.Id), context);
    }
}

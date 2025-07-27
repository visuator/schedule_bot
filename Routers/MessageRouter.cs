using Microsoft.Extensions.Caching.Memory;
using schedule_bot.Commands;
using schedule_bot.Extensions;
using schedule_bot.Menus.Impl;
using schedule_bot.Services;
using Telegram.Bot.Types;

namespace schedule_bot.Routers;

public interface IMessageRouter
{
    Task HandleMessage(Message message);
}
public class MessageRouter(
    IMemoryCache cache,
    ICommandRouter commandRouter,
    IFileHandlerRouter fileHandlerRouter,
    IUserRepository userRepository,
    MediatRMenuRouter menuRouter,
    MenuStorage storage
) : IMessageRouter
{
    public Task HandleMessage(Message message)
    {
        var user = userRepository.GetOrCreateDefault(new(message.GetUserId(), false));
        var context = new RequestContext()
        {
            User = user,
            Message = message,
            CallbackQuery = null,
            CurrentState = cache.Get<string>($"{user.Id}-current_state"),
        };
        if (message.IsCommand())
        {
            return commandRouter.HandleCommand(context);
        }
        if (message.Document is not null)
        {
            return fileHandlerRouter.HandleFile(context);
        }
        return menuRouter.Route(storage.GetAll(user.Id), context);
    }
}

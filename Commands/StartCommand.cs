using MediatR;
using schedule_bot.Extensions;
using schedule_bot.Menus;
using schedule_bot.Services;
using Telegram.Bot;

namespace schedule_bot.Commands;

public record StartCommand(RequestContext Context) : IRequest
{
    public class Handler(ITelegramBotClient client, MenuService menuService, MenuFactory factory) : IRequestHandler<StartCommand>
    {
        public async Task Handle(StartCommand request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request.Context.Message);
            var menu = factory.CreateStartMenu(request.Context.User.IsAdmin);
            menuService.Update(request.Context.User.Id, menu);
            await client.SendMessage(
                chatId: request.Context.Message.GetUserId(),
                text: Resources.GreetingMessage,
                replyMarkup: factory.CreateStartMenu(request.Context.User.IsAdmin).CreateMarkup(),
                cancellationToken: token
            );
        }
    }
}

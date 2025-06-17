using MediatR;
using schedule_bot.Extensions;
using schedule_bot.Services;
using Telegram.Bot;

namespace schedule_bot.Commands;

public record StartCommand(RequestContext Context) : IRequest
{
    public class Handler(ITelegramBotClient client, IUserRepository userRepository, MenuProvider menuProvider) : IRequestHandler<StartCommand>
    {
        public async Task Handle(StartCommand request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request.Context.Message);
            var menu = menuProvider.GetStartMenu(request.Context.User);
            userRepository.ChangeMenu(request.Context.User.Id, menu);
            await client.SendMessage(
                chatId: request.Context.Message.GetUserId(),
                text: Resources.GreetingMessage,
                replyMarkup: menu.GenerateKeyboard(),
                cancellationToken: token
            );
        }
    }
}

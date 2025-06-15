using MediatR;
using schedule_bot.Extensions;
using schedule_bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace schedule_bot.Commands;

public record AddVacationCommand(RequestContext Context) : IRequest
{
    public class Handler(ITelegramBotClient client, MenuProvider menuProvider) : IRequestHandler<AddVacationCommand>
    {
        public async Task Handle(AddVacationCommand request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request.Context.Message);
            var menu = menuProvider.GetDatePickerMenu(request.Context.Message.Date);
            await client.SendMessage(
                chatId: request.Context.Message.GetUserId(),
                text: Resources.GreetingMessage,
                replyMarkup: menu.GenerateKeyboard(),
                cancellationToken: token
            );
        }
    }
}

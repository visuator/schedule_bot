using MediatR;
using schedule_bot.Extensions;
using schedule_bot.Menus;
using schedule_bot.Services;
using Telegram.Bot;

namespace schedule_bot.Commands;

public record AddVacationCommand(RequestContext Context) : IRequest
{
    public class Handler(ITelegramBotClient client, IUserRepository userRepository) : IRequestHandler<AddVacationCommand>
    {
        public async Task Handle(AddVacationCommand request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request.Context.Message);
            var menu = new DatePickerMenu(request.Context.Message.Date);
            userRepository.ChangeMenu(request.Context.User.Id, menu);
            await client.SendMessage(
                chatId: request.Context.Message.GetUserId(),
                text: "Выбери начальную дату",
                replyMarkup: menu.GenerateKeyboard(),
                cancellationToken: token
            );
        }
    }
}

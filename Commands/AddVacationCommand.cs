using MediatR;
using schedule_bot.Extensions;
using schedule_bot.Menus.Impl;
using Telegram.Bot;

namespace schedule_bot.Commands;

public record AddVacationCommand(RequestContext Context) : IRequest
{
    public class Handler(ITelegramBotClient client, MenuStorage storage, MenuFactory factory) : IRequestHandler<AddVacationCommand>
    {
        public async Task Handle(AddVacationCommand request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request.Context.Message);
            var menu = factory.CreateDatePickerMenu(request.Context.Message.Date);
            //todo: use message id instead of replacement
            storage.Switch(request.Context.User.Id, menu);
            await client.SendMessage(
                chatId: request.Context.Message.GetUserId(),
                text: Resources.SelectDate,
                replyMarkup: menu.ToMarkup(),
                cancellationToken: token
            );
        }
    }
}

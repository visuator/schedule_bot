using MediatR;
using Microsoft.Extensions.Caching.Memory;
using schedule_bot.Extensions;
using schedule_bot.Menus.Impl;
using schedule_bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Commands;

public record DatePickerCommand(DateTime Date, RequestContext Context) : IRequest
{
    public class Handler(IMemoryCache cache, ITelegramBotClient client, MenuFactory factory, MenuStorage storage, IVacationService vacationService) : IRequestHandler<DatePickerCommand>
    {
        public async Task Handle(DatePickerCommand request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request.Context.CurrentState);
            ArgumentNullException.ThrowIfNull(request.Context.CallbackQuery?.Message);

            if (request.Context.CurrentState == "set_vacation")
            {
                var value = cache.Get<VacationState>($"{request.Context.User.Id}-vacation_state");
                if (value is null)
                {
                    cache.Set($"{request.Context.User.Id}-vacation_state", new VacationState() { StartDate = request.Date });
                    var menu = factory.CreateDatePickerMenu(request.Date);
                    storage.Switch(request.Context.User.Id, menu);
                    await client.EditMessageReplyMarkup(
                        chatId: request.Context.CallbackQuery.Message.GetUserId(),
                        messageId: request.Context.CallbackQuery.Message.Id,
                        replyMarkup: menu.ToMarkup() as InlineKeyboardMarkup,
                        cancellationToken: token
                    );
                }
                else
                {
                    value.EndDate = request.Date;
                    await client.DeleteMessage(
                        chatId: request.Context.CallbackQuery.Message.GetUserId(),
                        messageId: request.Context.CallbackQuery.Message.Id,
                        cancellationToken: token
                    );
                    vacationService.AddVacation(new(value.StartDate, value.EndDate));
                    await client.SendMessage(
                        chatId: request.Context.CallbackQuery.Message.GetUserId(),
                        text: Resources.AddVacationSuccess,
                        cancellationToken: token
                    );
                    cache.Remove($"{request.Context.User.Id}-vacation_state");
                }
            }
            await client.AnswerCallbackQuery(
                callbackQueryId: request.Context.CallbackQuery.Id,
                cancellationToken: token
            );
        }
    }
}

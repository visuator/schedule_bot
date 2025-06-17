using MediatR;
using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class DatePickerMenu : InlineMenu
{
    private static readonly InlineKeyboardButton NextPage = new("\u2192") { CallbackData = "next" };

    public DatePickerMenu(MenuContext menuContext, IMediator mediator) : base(menuContext, mediator)
    {
        var from = DateTime.Parse(menuContext.Data["from"]);
        var count = DateTime.DaysInMonth(from.Year, from.Month);
        var dates = Enumerable.Range(from.Day, count - from.Day).Select(x => $"{x}").ToHashSet();

        Buttons.AddRange(
            dates
                .Select(x => new InlineKeyboardButton(x) { CallbackData = x })
                .Chunk(3)
                .Append([NextPage])
        );

        Route(context =>
        {
            ArgumentNullException.ThrowIfNull(context.CallbackQuery?.Data);
            return dates.Contains(context.CallbackQuery.Data);
        }, context => new DatePickerCommand(context));
        Route("back", context => new DatePickerNextPageCommand(context));
    }
}

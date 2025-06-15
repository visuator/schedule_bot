using MediatR;
using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class DatePickerMenu(DateTime from, IMediator mediator, IEnumerable<InlineMenu> children) : InlineMenu(mediator, children)
{
    private static readonly InlineKeyboardButton NextPage = new("\u2192");

    public DatePickerMenu(DateTime from, IMediator mediator) : this(from, mediator, []) { }
    /*protected override InlineKeyboardButton[][] Buttons
    {
        get
        {
            var daysCount = DateTime.DaysInMonth(from.Year, from.Month);
            var currentDay = from.Day;
            var buttons = Enumerable.Range(currentDay, daysCount - currentDay)
                .Select(x => new InlineKeyboardButton($"{x}"))
                .Chunk(3)
                .Append([NextPage])
                .ToArray();
            return buttons;
        }
    }
    protected override Dictionary<string, Func<RequestContext, IRequest>> Routes => new()
    {

    };*/
}

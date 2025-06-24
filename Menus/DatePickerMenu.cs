using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class DatePickerMenuSnapshot : MenuSnapshot
{
    public DateTime Date { get; set; }
}
public class DatePickerMenu : InlineMenu
{
    private readonly DateTime _date;
    public DatePickerMenu(DateTime date)
    {
        _date = date;
        var daysCount = DateTime.DaysInMonth(_date.Year, _date.Month);
        var remainedDayNumbers = Enumerable.Range(_date.Day, daysCount - _date.Day)
            .Select(x => $"{x}")
            .ToHashSet();

        Rows.AddRange(remainedDayNumbers
            .Select(x => new InlineKeyboardButton(x) { CallbackData = x })
            .Chunk(3)
            .Append([NextPage])
        );
        Routes[@"\d+"] = context => new DatePickerCommand(context);
    }
    public DatePickerMenu(DatePickerMenuSnapshot snapshot) : this(snapshot.Date) { }
    public override MenuSnapshot CreateSnapshot() => new DatePickerMenuSnapshot() { Date = _date };
}

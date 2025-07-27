using schedule_bot.Commands;
using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus;

public class DatePickerMenu : InlineMenu
{
    public DateTime Date { get; }

    public DatePickerMenu(DateTime date)
    {
        Date = date;

        Rows.AddRange(Enumerable.Range(0, Math.Max(DateTime.DaysInMonth(date.Year, date.Month), DateTime.DaysInMonth(date.Year, date.Month + 1)))
            .Select(x => date.AddDays(x))
            .Select(x => new MediatRInlineMenuButton($"{x.Day}", $"{x.Day}", context => new DatePickerCommand(x, context)))
            .Chunk(3)
        );
    }
}

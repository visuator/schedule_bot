using schedule_bot.Commands;
using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus;

public class DatePickerMenu : InlineMenu
{
    public DateTime Date { get; }

    public DatePickerMenu(DateTime date)
    {
        Date = date;

        var day = date.Day;
        var daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

        
        Rows.AddRange(Enumerable.Range(day, daysInMonth - day)
            .Select(x => $"{x}")
            .Select(x => new MediatRInlineMenuButton(x, x, context => new DatePickerCommand(context)))
            .Chunk(3)
        );
    }
}

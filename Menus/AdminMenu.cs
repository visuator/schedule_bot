using schedule_bot.Commands;
using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus;

public class AdminMenu : ReplyMenu
{
    public AdminMenu()
    {
        HasRow(row =>
        {
            row.HasMediatRButton(button =>
            {
                button.WithText(Resources.AddVacation);
                button.Routes(context => new AddVacationCommand(context));
            });
            row.HasMediatRButton(button =>
            {
                button.WithText(Resources.ImportSchedule);
                button.Routes(context => new ImportScheduleCommand(context));
            });
        });
    }
}

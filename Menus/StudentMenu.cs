using schedule_bot.Commands;
using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus;

public class StudentMenu : ReplyMenu
{
    public StudentMenu()
    {
        HasRow(row =>
        {
            row.HasMediatRButton(button =>
            {
                button.WithText(Resources.TodayScheduling);
                button.Routes(context => new TodayScheduleCommand(context));
            });
            row.HasMediatRButton(button =>
            {
                button.WithText(Resources.TomorrowScheduling);
                button.Routes(context => new TomorrowScheduleCommand(context));
            });
            row.HasMediatRButton(button =>
            {
                button.WithText(Resources.FullScheduling);
                button.Routes(context => new FullScheduleCommand(context));
            });
        });
        HasRow(row =>
        {
            row.HasMediatRButton(button =>
            {
                button.WithText(Resources.ShowList);
                button.Routes(context => new ShowListCommand(context));
            });
            row.HasMediatRButton(button =>
            {
                button.WithText(Resources.AddTask);
                button.Routes(context => new AddTaskCommand(context));
            });
            row.HasMediatRButton(button =>
            {
                button.WithText(Resources.UpdateTaskStatus);
                button.Routes(context => new UpdateTaskStatusCommand(context));
            });
        });
    }
}

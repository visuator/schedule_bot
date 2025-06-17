using MediatR;
using schedule_bot.Commands;
using schedule_bot.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class StudentMenu : ReplyMenu
{
    public StudentMenu(MenuContext menuContext, IMediator mediator) : base(menuContext, mediator)
    {
        AddButtonRow(new KeyboardButton(Resources.TodayScheduling), new KeyboardButton(Resources.TomorrowScheduling), new KeyboardButton(Resources.FullScheduling));
        AddButtonRow(new KeyboardButton(Resources.ShowList), new KeyboardButton(Resources.AddTask), new KeyboardButton(Resources.UpdateTaskStatus));

        Route(Resources.TodayScheduling, context => new TodayScheduleCommand(context));
        Route(Resources.TomorrowScheduling, context => new TomorrowScheduleCommand(context));
        Route(Resources.FullScheduling, context => new FullScheduleCommand(context));
        Route(Resources.ShowList, context => new ShowListCommand(context));
        Route(Resources.AddTask, context => new AddTaskCommand(context));
        Route(Resources.UpdateTaskStatus, context => new UpdateTaskStatusCommand(context));
        Route(Resources.SetupNotifications, context => new SetupNotificationsCommand(context));
    }
}

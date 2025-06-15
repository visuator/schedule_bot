using MediatR;
using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class StudentMenu(IMediator mediator, IEnumerable<ReplyMenu> children) : ReplyMenu(mediator, children)
{
    public StudentMenu(IMediator mediator) : this(mediator, [])
    {

    }
    /*protected override KeyboardButton[][] Buttons =>
    [
        [
            new KeyboardButton(Resources.TodayScheduling),
            new KeyboardButton(Resources.TomorrowScheduling),
            new KeyboardButton(Resources.FullScheduling)
        ],
        [
            new KeyboardButton(Resources.ShowList),
            new KeyboardButton(Resources.AddTask),
            new KeyboardButton(Resources.UpdateTaskStatus)
        ],
        [new KeyboardButton(Resources.SetupNotifications)]
    ];

    protected override Dictionary<string, Func<RequestContext, IRequest>> Routes => new()
    {
        { Resources.TodayScheduling, context => new TodayScheduleCommand(context) },
        { Resources.TomorrowScheduling, context => new TomorrowScheduleCommand(context) },
        { Resources.FullScheduling, context => new FullScheduleCommand(context) },
        { Resources.ShowList, context => new ShowListCommand(context) },
        { Resources.AddTask, context => new AddTaskCommand(context) },
        { Resources.UpdateTaskStatus, context => new UpdateTaskStatusCommand(context) },
        { Resources.SetupNotifications, context => new SetupNotificationsCommand(context) }
    };*/
}

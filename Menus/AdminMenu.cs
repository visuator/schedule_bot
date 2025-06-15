using MediatR;
using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class AdminMenu(IMediator mediator, IEnumerable<ReplyMenu> children) : ReplyMenu(mediator, children)
{
    public AdminMenu(IMediator mediator) : this(mediator, [])
    {
        Buttons.Add([new KeyboardButton(Resources.AddVacation)]);
        Routes.Add(Resources.AddVacation, context => new AddVacationCommand(context));
    }
}

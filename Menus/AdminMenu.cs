using MediatR;
using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class AdminMenu : ReplyMenu
{
    public AdminMenu(MenuContext menuContext, IMediator mediator) : base(menuContext, mediator)
    {
        AddButtonRow(new KeyboardButton(Resources.AddVacation));

        Route(Resources.AddVacation, context => new AddVacationCommand(context));
    }
}

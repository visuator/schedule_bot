using System.Globalization;
using MediatR;
using schedule_bot.Entities;
using schedule_bot.Menus;

namespace schedule_bot.Services;

public class MenuProvider(IMediator mediator)
{
    public IMenu GetStartMenu(User user)
    {
        var context = new MenuContext([], "start_menu");
        return new CombinedReplyMenu(
            user.IsAdmin
                ? [new StudentMenu(context, mediator), new AdminMenu(context, mediator)]
                : [new StudentMenu(context, mediator)],
            context,
            mediator
        );
    }

    public IMenu GetDatePickerMenu(DateTime from)
    {
        var context = new MenuContext(new()
        {
            ["from"] = from.ToString(CultureInfo.InvariantCulture)
        }, "date_picker_menu");
        return new DatePickerMenu(context, mediator);
    }

    public IMenu Restore(User user)
    {
        var context = new MenuContext(user.MenuSnapshot.Data, user.MenuSnapshot.Name);
        IMenu instance = user.MenuSnapshot.Name switch
        {
            "admin_menu" => new AdminMenu(context, mediator),
            "student_menu" => new StudentMenu(context, mediator),
            "start_menu" => new CombinedReplyMenu(
                user.IsAdmin
                    ? [new StudentMenu(context, mediator), new AdminMenu(context, mediator)]
                    : [new StudentMenu(context, mediator)],
                context,
                mediator
            ),
            "date_picker_menu" => new DatePickerMenu(context, mediator),
            _ => throw new Exception()
        };
        return instance;
    }
}

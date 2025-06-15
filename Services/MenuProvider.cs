using MediatR;
using schedule_bot.Entities;
using schedule_bot.Menus;

namespace schedule_bot.Services;

public class MenuProvider(IMediator mediator)
{
    private static readonly Func<IMediator, User, IMenu> _startMenuFactory = (mediator, user) => user.IsAdmin ? new StudentMenu(mediator, [new AdminMenu(mediator)]) : new StudentMenu(mediator);
    private static readonly Func<DateTime, IMediator, IMenu> _datePickerMenuFactory = (from, mediator) => new DatePickerMenu(from, mediator);
    private static readonly Dictionary<string, Func<IMediator, User, IMenu>> _restoreFactories = new()
    {
        { "admin_menu", (mediator, user) => new AdminMenu(mediator) },
        { "student_menu", (mediator, user) => new StudentMenu(mediator) },
        { "date_picker_menu", (mediator, user) => _datePickerMenuFactory(DateTime.Parse(user.LastMenu.Data["from"]), mediator) },
        { "start_menu", (mediator, user) => _startMenuFactory(mediator, user) }
    };

    public IMenu Restore(User user) => _restoreFactories[user.LastMenu.Name](mediator, user);
    public IMenu GetStartMenu(User user) => _startMenuFactory(mediator, user);
    public IMenu GetDatePickerMenu(DateTime from) => _datePickerMenuFactory(from, mediator);
}

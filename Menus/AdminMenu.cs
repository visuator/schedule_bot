using schedule_bot.Commands;

namespace schedule_bot.Menus;

public class AdminMenuSnapshot : MenuSnapshot { }
public class AdminMenu : ReplyMenu
{
    public AdminMenu()
    {
        Append(Resources.AddVacation);
        Routes["\ud83d\udcc6 Добавить выходные дни"] = context => new AddVacationCommand(context);
    }
    public AdminMenu(AdminMenuSnapshot snapshot) : this() { }
    public override MenuSnapshot CreateSnapshot() => new AdminMenuSnapshot();
}

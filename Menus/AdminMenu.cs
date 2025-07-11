using schedule_bot.Commands;

namespace schedule_bot.Menus;

public class AdminMenuSnapshot : MenuSnapshot;
public class AdminMenu : ReplyMenu
{
    public AdminMenu()
    {
        Append(Resources.AddVacation);
        Append(Resources.ImportSchedule);
        Routes[Resources.AddVacation] = context => new AddVacationCommand(context);
        Routes[Resources.ImportSchedule] = context => new ImportCommand(context);
    }
    public AdminMenu(AdminMenuSnapshot snapshot) : this() { }
    public override MenuSnapshot CreateSnapshot() => new AdminMenuSnapshot();
}

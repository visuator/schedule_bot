using schedule_bot.Menus.Abstract;
using schedule_bot.Menus.Impl;

namespace schedule_bot.Menus;

public class StartMenu : ReplyMenu
{
    public bool IsAdmin { get; }

    public StartMenu(MenuFactory factory, bool isAdmin)
    {
        IsAdmin = isAdmin;

        Append(factory.CreateStudentMenu());
        if (isAdmin)
            Append(factory.CreateAdminMenu());
    }
}


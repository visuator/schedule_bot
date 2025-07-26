using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus.Impl;

public class MenuFactory
{
    public ReplyMenu CreateAdminMenu() => new AdminMenu();

    public ReplyMenu CreateStudentMenu() => new StudentMenu();

    public ReplyMenu CreateStartMenu(bool isAdmin) => new StartMenu(this, isAdmin);

    public InlineMenu CreateDatePickerMenu(DateTime date) => new DatePickerMenu(date);

    public InlineMenu CreateSubjectMenu(string[] subjects) => new SubjectMenu(subjects);
}

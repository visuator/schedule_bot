namespace schedule_bot.Menus;

public class StartMenuSnapshot : MenuSnapshot
{
    public bool IsAdmin { get; set; }
}
public class StartMenu : ReplyMenu
{
    private readonly bool _isAdmin;
    public StartMenu(bool isAdmin, MenuFactory factory)
    {
        _isAdmin = isAdmin;
        Append(factory.CreateStudentMenu());
        if (_isAdmin)
            Append(factory.CreateAdminMenu());
    }
    public StartMenu(StartMenuSnapshot snapshot, MenuFactory factory) : this(snapshot.IsAdmin, factory) { }
    public override MenuSnapshot CreateSnapshot() => new StartMenuSnapshot() { IsAdmin = _isAdmin };
}


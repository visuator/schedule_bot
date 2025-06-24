namespace schedule_bot.Menus;

public class TestComposeMenuSnapshot : MenuSnapshot
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
}
public class TestComposeMenu : InlineMenu
{
    private readonly DateTime _from;
    private readonly DateTime _to;

    public TestComposeMenu(DateTime from, DateTime to, MenuFactory factory)
    {
        _from = from;
        _to = to;

        Append(factory.CreateDatePickerMenu(_from));
        Append(factory.CreateDatePickerMenu(_to));
    }
    public TestComposeMenu(TestComposeMenuSnapshot snapshot, MenuFactory factory) : this(snapshot.From, snapshot.To, factory) { }
    public override MenuSnapshot CreateSnapshot() => new TestComposeMenuSnapshot() { From = _from, To = _to };
}

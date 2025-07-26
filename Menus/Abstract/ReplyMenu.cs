using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus.Abstract;

public abstract class ReplyMenuButton(string pattern, string text) : IButton
{
    public string Pattern => pattern;
    public string Text => text;
}
public abstract class ReplyMenu : MenuBase<ReplyMenuButton>
{
    protected void Append(ReplyMenu menu)
    {
        Rows.AddRange(menu.Rows);
    }

    protected void Prepend(ReplyMenu menu)
    {
        Rows.InsertRange(0, menu.Rows);
    }

    protected void HasRow(Action<RowBuilder> action)
    {
        var builder = new RowBuilder();
        action(builder);
        Rows.Add([.. builder.Buttons]);
    }
    public override ReplyMarkup ToMarkup() => new ReplyKeyboardMarkup(Rows.Select(x => x.Select(x => new KeyboardButton(x.Text))));
}
public class RowBuilder
{
    public List<ReplyMenuButton> Buttons { get; } = [];
}

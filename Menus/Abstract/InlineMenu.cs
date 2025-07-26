using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus.Abstract;

public abstract class InlineMenuButton(string pattern, string text) : IButton
{
    public string Pattern => pattern;
    public string Text => text;
}
public abstract class InlineMenu : MenuBase<InlineMenuButton>
{
    public override ReplyMarkup ToMarkup() => new InlineKeyboardMarkup(Rows.Select(x => x.Select(x => new InlineKeyboardButton(x.Text) { CallbackData = x.Pattern })));
}

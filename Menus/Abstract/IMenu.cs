using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus.Abstract;

public interface IMenu
{
    string Name { get; }
    IReadOnlyCollection<IButton> Buttons { get; }
    ReplyMarkup ToMarkup();
}
public interface IButton
{
    string Pattern { get; }
    string Text { get; }
}

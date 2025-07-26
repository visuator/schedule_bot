using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus.Abstract;

public abstract class MenuBase<TButton> : IMenu where TButton : IButton
{
    protected List<TButton[]> Rows { get; } = [];
    public IReadOnlyCollection<IButton> Buttons => [.. Rows.SelectMany(x => x).Cast<IButton>()];

    public string Name => GetType().Name;

    public abstract ReplyMarkup ToMarkup();
}

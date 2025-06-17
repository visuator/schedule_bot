using MediatR;
using schedule_bot.Commands;
using schedule_bot.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;


public interface IMenu
{
    ReplyMarkup GenerateKeyboard();
    Task Route(RequestContext context);
    MenuSnapshot Save();
}

public record MenuContext(Dictionary<string, string> Data, string Name);
public abstract class MenuBase<TButton>(MenuContext menuContext, IMediator mediator) : IMenu
{
    private List<(Func<RequestContext, bool> Matches, Func<RequestContext, IRequest> Factory)> _routes { get; } = [];

    protected List<TButton[]> Buttons { get; } = [];
    protected void AddButtonRow(params TButton[] button) => Buttons.Add(button);
    protected void Route(Func<RequestContext, bool> matches, Func<RequestContext, IRequest> factory) => _routes.Add((matches, factory));

    public abstract ReplyMarkup GenerateKeyboard();

    public void Append(MenuBase<TButton> other)
    {
        Buttons.AddRange(other.Buttons);
        _routes.AddRange(other._routes);
    }

    public Task Route(RequestContext context)
    {
        foreach (var (matches, factory) in _routes)
            if (matches(context))
                return mediator.Send(factory(context));
        return Task.CompletedTask;
    }

    public MenuSnapshot Save() => new() { Data = menuContext.Data, Name = menuContext.Name };
}
public class CombinedReplyMenu : ReplyMenu
{
    public CombinedReplyMenu(IEnumerable<ReplyMenu> children, MenuContext menuContext, IMediator mediator) : base(menuContext, mediator)
    {
        foreach (var child in children)
        {
            Append(child);
        }
    }
}
public abstract class ReplyMenu(MenuContext menuContext, IMediator mediator) : MenuBase<KeyboardButton>(menuContext, mediator)
{
    public override ReplyMarkup GenerateKeyboard() => new ReplyKeyboardMarkup();
    protected void Route(string text, Func<RequestContext, IRequest> factory)
    {
        Route(context =>
        {
            ArgumentNullException.ThrowIfNull(context.Message?.Text);
            return text.Equals(context.Message.Text, StringComparison.Ordinal);
        }, factory);
    }
}

public abstract class InlineMenu(MenuContext menuContext, IMediator mediator) : MenuBase<InlineKeyboardButton>(menuContext, mediator)
{
    public override ReplyMarkup GenerateKeyboard() => new InlineKeyboardMarkup(Buttons);
    protected void Route(string callbackQueryText, Func<RequestContext, IRequest> factory)
    {
        Route(context =>
        {
            ArgumentNullException.ThrowIfNull(context.CallbackQuery?.Data);
            return callbackQueryText.Equals(context.CallbackQuery.Data, StringComparison.Ordinal);
        }, factory);
    }
}

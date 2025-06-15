using MediatR;
using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public interface IMenu
{
    ReplyMarkup GenerateKeyboard();
    Task Route(RequestContext context);
}
public abstract class Menu<TButton, TRouteKey> : IMenu where TRouteKey : notnull
{
    protected Menu(IMediator mediator, IEnumerable<Menu<TButton, TRouteKey>> children)
    {
        Mediator = mediator;
        foreach (var child in children)
        {
            Buttons.AddRange(child.Buttons);
            foreach (var (k,v) in child.Routes)
            {
                Routes.Add(k, v);
            }
        }
    }
    protected IMediator Mediator { get; }
    protected List<TButton[]> Buttons { get; } = [];
    protected Dictionary<TRouteKey, Func<RequestContext, IRequest>> Routes { get; } = [];
    public abstract ReplyMarkup GenerateKeyboard();
    public abstract Task Route(RequestContext context);
}
public abstract class ReplyMenu(IMediator mediator, IEnumerable<Menu<KeyboardButton, string>> children) : Menu<KeyboardButton, string>(mediator, children)
{
    public override ReplyMarkup GenerateKeyboard() => new ReplyKeyboardMarkup(Buttons);
    public override async Task Route(RequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context.Message?.Text);
        if (Routes.TryGetValue(context.Message.Text, out var factory))
        {
            await Mediator.Send(factory(context));
        }
    }
}
public abstract class InlineMenu(IMediator mediator, IEnumerable<Menu<InlineKeyboardButton, string>> children) : Menu<InlineKeyboardButton, string>(mediator, children)
{
    public override ReplyMarkup GenerateKeyboard() => new InlineKeyboardMarkup(Buttons);
    public override async Task Route(RequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context.Message?.Text);
        if (Routes.TryGetValue(context.Message.Text, out var factory))
        {
            await Mediator.Send(factory(context));
        }
    }
}

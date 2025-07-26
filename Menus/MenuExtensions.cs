using MediatR;
using schedule_bot.Commands;
using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus;

public static class MenuExtensions
{
    public static RowBuilder HasMediatRButton(this RowBuilder rowBuilder, Action<MediatRButtonBuilder> action)
    {
        var builder = new MediatRButtonBuilder();
        action(builder);
        rowBuilder.Buttons.Add(builder.Build());
        return rowBuilder;
    }
}
public class MediatRButtonBuilder
{
    private Func<RequestContext, IRequest>? _factory;
    private string? _text;
    private string? _pattern;

    public MediatRButtonBuilder WithText(string text)
    {
        _text = text;
        _pattern = text;
        return this;
    }
    public MediatRButtonBuilder Routes(Func<RequestContext, IRequest> factory)
    {
        _factory = factory;
        return this;
    }
    public ReplyMenuButton Build()
    {
        ArgumentNullException.ThrowIfNull(_pattern);
        ArgumentNullException.ThrowIfNull(_text);
        ArgumentNullException.ThrowIfNull(_factory);

        return new MediatRReplyMenuButton(_pattern, _text, _factory);
    }
}
public class MediatRInlineMenuButton(string pattern, string text, Func<RequestContext, IRequest> factory) : InlineMenuButton(pattern, text)
{
    public Func<RequestContext, IRequest> Factory { get; } = factory;
}
public class MediatRReplyMenuButton(string pattern, string text, Func<RequestContext, IRequest> factory) : ReplyMenuButton(pattern, text)
{
    public Func<RequestContext, IRequest> Factory { get; } = factory;
}
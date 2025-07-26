using System.Text.RegularExpressions;
using MediatR;
using schedule_bot.Commands;
using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus.Impl;

public class MediatRMenuRouter(IMediator mediator)
{
    public Task Route(IEnumerable<IMenu> instances, RequestContext context)
    {
        foreach (var menu in instances)
        {
            var text = context.Message?.Text ?? context.CallbackQuery?.Data;
            ArgumentException.ThrowIfNullOrEmpty(text);
            
            foreach (var route in menu.Buttons.OfType<MediatRReplyMenuButton>())
            {
                if (text == route.Pattern || new Regex(route.Pattern).IsMatch(text))
                    return mediator.Send(route.Factory(context));
            }
            foreach (var route in menu.Buttons.OfType<MediatRInlineMenuButton>())
            {
                if (text == route.Pattern || new Regex(route.Pattern).IsMatch(text))
                    return mediator.Send(route.Factory(context));
            }
        }
        throw new InvalidOperationException();
    }
}

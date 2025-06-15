using MediatR;
using schedule_bot.Commands;

namespace schedule_bot.Routers;

public interface ICommandRouter
{
    Task HandleCommand(RequestContext context);
}
public class CommandRouter(IMediator mediator) : ICommandRouter
{
    public async Task HandleCommand(RequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context.Message?.Text);
        if (context.Message.Text.StartsWith("/start"))
        {
            await mediator.Send(new StartCommand(context));
        }
    }
}

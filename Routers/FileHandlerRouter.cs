using MediatR;
using schedule_bot.Commands;

namespace schedule_bot.Routers;

public interface IFileHandlerRouter
{
    Task HandleFile(RequestContext context);
}
public class FileHandlerRouter(IMediator mediator) : IFileHandlerRouter
{
    public async Task HandleFile(RequestContext context)
    {
        ArgumentNullException.ThrowIfNull(context.Message?.Document?.FileName);
        switch (Path.GetExtension(context.Message.Document.FileName))
        {
            case ".xlsx":
                await mediator.Send(new ImportCommand(context));
                break;
        }
    }
}

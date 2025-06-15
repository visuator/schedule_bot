using MediatR;

namespace schedule_bot.Commands;

public record SetupNotificationsCommand(RequestContext context) : IRequest
{
    public class Handler : IRequestHandler<SetupNotificationsCommand>
    {
        public async Task Handle(SetupNotificationsCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

using MediatR;

namespace schedule_bot.Commands;

public record UpdateTaskStatusCommand(RequestContext context) : IRequest
{
    public class Handler : IRequestHandler<UpdateTaskStatusCommand>
    {
        public async Task Handle(UpdateTaskStatusCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

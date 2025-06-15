using MediatR;

namespace schedule_bot.Commands;

public record AddTaskCommand(RequestContext context) : IRequest
{
    public class Handler : IRequestHandler<AddTaskCommand>
    {
        public async Task Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

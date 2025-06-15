using MediatR;

namespace schedule_bot.Commands;

public record FullScheduleCommand(RequestContext context) : IRequest
{
    public class Handler : IRequestHandler<FullScheduleCommand>
    {
        public async Task Handle(FullScheduleCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

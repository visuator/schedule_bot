using MediatR;

namespace schedule_bot.Commands;

public record TomorrowScheduleCommand(RequestContext context) : IRequest
{
    public class Handler : IRequestHandler<TomorrowScheduleCommand>
    {
        public async Task Handle(TomorrowScheduleCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

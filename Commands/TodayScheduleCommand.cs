using MediatR;

namespace schedule_bot.Commands;

public record TodayScheduleCommand(RequestContext context) : IRequest
{
    public class Handler : IRequestHandler<TodayScheduleCommand>
    {
        public async Task Handle(TodayScheduleCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

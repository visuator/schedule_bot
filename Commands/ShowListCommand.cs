using MediatR;

namespace schedule_bot.Commands;

public record ShowListCommand(RequestContext context) : IRequest
{
    public class Handler : IRequestHandler<ShowListCommand>
    {
        public async Task Handle(ShowListCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

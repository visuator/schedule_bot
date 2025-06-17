using MediatR;

namespace schedule_bot.Commands;

public record DatePickerCommand(RequestContext Context) : IRequest
{
    public class Handler : IRequestHandler<DatePickerCommand>
    {
        public async Task Handle(DatePickerCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

using MediatR;

namespace schedule_bot.Commands;

public record DatePickerNextPageCommand(RequestContext Context) : IRequest
{
    public class Handler : IRequestHandler<DatePickerNextPageCommand>
    {
        public async Task Handle(DatePickerNextPageCommand request, CancellationToken cancellationToken)
        {

        }
    }
}

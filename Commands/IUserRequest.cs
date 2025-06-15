using MediatR;

namespace schedule_bot.Commands;

public interface IUserRequest : IRequest
{
    public RequestContext Context { get; }
}

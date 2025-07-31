using MediatR;
using schedule_bot.Menus.Impl;
using schedule_bot.Services;
using Telegram.Bot;

namespace schedule_bot.Commands;

public record AddTaskCommand(RequestContext Context) : IRequest
{
    public class Handler(ITelegramBotClient client, IScheduleRepository subjectRepository, MenuFactory factory) : IRequestHandler<AddTaskCommand>
    {
        public async Task Handle(AddTaskCommand request, CancellationToken cancellationToken)
        {
            ArgumentNullException.ThrowIfNull(request.Context.Message);
            var subjects = subjectRepository.GetSubjectNames();
            var menu = factory.CreateSubjectMenu(subjects);
            await client.SendMessage(
                chatId: request.Context.Message.Chat.Id,
                text: Resources.SelectSubject,
                replyMarkup: menu.ToMarkup(),
                cancellationToken: cancellationToken
            );
        }
    }
}

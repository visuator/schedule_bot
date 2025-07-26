using MediatR;
using MediatR.Pipeline;
using schedule_bot.Common;
using schedule_bot.Extensions;
using schedule_bot.Services;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace schedule_bot.Commands;

public record ImportScheduleCommand(RequestContext Context) : IRequest
{
    public class Handler(ITelegramBotClient client, IScheduleImportService importService) : IRequestHandler<ImportScheduleCommand>
    {
        public async Task Handle(ImportScheduleCommand request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request.Context.Message);
            if (!request.Context.User.IsAdmin)
                throw new UserIsNotAdminException(request.Context.Message.GetUserId());
            var file = await client.GetFile(request.Context.Message.GetFileId(), cancellationToken: token);
            using var memoryStream = new MemoryStream();
            await client.DownloadFile(file, memoryStream, token);
            importService.Import(memoryStream, request.Context.Message.Date);
        }
    }
}
public class ExportCommandExceptionHandler(ITelegramBotClient client) : IRequestExceptionAction<ImportScheduleCommand, UserIsNotAdminException>
{
    public async Task Execute(ImportScheduleCommand request, UserIsNotAdminException exception, CancellationToken token)
    {
        await client.SendMessage(
            chatId: exception.UserId,
            text: Resources.UserIsNotAdmin,
            cancellationToken: token
        );
    }
}

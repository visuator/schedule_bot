using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace schedule_bot.Extensions;

public static class TelegramExtensions
{
    public static bool IsCommand(this Message message) => message.Entities?.Length == 1 && message.Entities[0].Type == MessageEntityType.BotCommand && message.Text is not null;
    public static long GetUserId(this Message message)
    {
        ArgumentNullException.ThrowIfNull(message.From);
        return message.Chat.Id;
    }
    public static string GetFileId(this Message message)
    {
        ArgumentNullException.ThrowIfNull(message.Document);
        return message.Document.FileId;
    }
}

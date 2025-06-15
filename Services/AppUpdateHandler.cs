using System.Globalization;
using schedule_bot.Routers;
using Telegram.Bot;
using Telegram.Bot.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace schedule_bot.Services;

public class AppUpdateHandler(IMessageRouter messageRouter, ICallbackQueryRouter callbackQueryRouter) : IUpdateHandler
{
    private static void SetCulture(string? languageCode)
    {
        ArgumentException.ThrowIfNullOrEmpty(languageCode);
        Resources.Culture = CultureInfo.GetCultureInfoByIetfLanguageTag(languageCode);
    }

    public Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        switch (update.Type)
        {
            case UpdateType.Message:
                ArgumentNullException.ThrowIfNull(update.Message);
                SetCulture(update.Message.From?.LanguageCode);
                return messageRouter.HandleMessage(update.Message);
            case UpdateType.CallbackQuery:
                ArgumentNullException.ThrowIfNull(update.CallbackQuery);
                SetCulture(update.CallbackQuery.From.LanguageCode);
                return callbackQueryRouter.HandleCallbackQuery(update.CallbackQuery);
            default:
                return Task.CompletedTask;
        }
        return Task.CompletedTask;
    }

    public Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, HandleErrorSource source, CancellationToken cancellationToken) => Task.CompletedTask;
}

using Microsoft.Extensions.Options;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace schedule_bot.Services;

public class ReceiverService(ITelegramBotClient client, IOptions<ReceiverOptions> receiverOptions, IUpdateHandler updateHandler)
{
    public Task ReceiveAsync(CancellationToken stoppingToken) => client.ReceiveAsync(updateHandler, receiverOptions.Value, stoppingToken);
}
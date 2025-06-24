using System.Reflection;
using LiteDB;
using Microsoft.Extensions.Options;
using schedule_bot.Configuration;
using schedule_bot.Menus;
using schedule_bot.Routers;
using schedule_bot.Services;
using Telegram.Bot;
using Telegram.Bot.Polling;

var builder = Host.CreateDefaultBuilder(args);
builder.ConfigureServices((context, services) =>
{
    services.AddLogging();
    services.AddMediatR(x =>
    {
        x.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
    });
    services.AddHttpClient("telegram_bot_client").RemoveAllLoggers()
        .AddTypedClient<ITelegramBotClient>((httpClient, sp) =>
        {
            var botConfiguration = sp.GetService<IOptions<BotConfiguration>>()?.Value;
            ArgumentNullException.ThrowIfNull(botConfiguration);
            var options = new TelegramBotClientOptions(botConfiguration.BotToken);
            return new TelegramBotClient(options, httpClient);
        });
    services.Configure<ReceiverOptions>(x =>
    {
        x.DropPendingUpdates = true;
        x.AllowedUpdates = [];
    });
    services.Configure<AdminConfiguration>(context.Configuration.GetSection(nameof(AdminConfiguration)));
    services.Configure<BotConfiguration>(context.Configuration.GetSection(nameof(BotConfiguration)));
    services.AddSingleton(_ => new LiteDatabase(context.Configuration.GetConnectionString("Database")));
    services.AddScoped<IUserRepository, UserRepository>();
    services.AddScoped<IScheduleImportService, ExcelScheduleImportService>();
    services.AddScoped<IScheduleRepository, ScheduleRepository>();
    services.AddScoped<ReceiverService>();
    services.AddScoped<IMessageRouter, MessageRouter>();
    services.AddScoped<ICallbackQueryRouter, CallbackQueryRouter>();
    services.AddScoped<ICommandRouter, CommandRouter>();
    services.AddScoped<IFileHandlerRouter, FileHandlerRouter>();
    services.AddScoped<IUpdateHandler, AppUpdateHandler>();
    services.AddScoped<MenuFactory>();
    services.AddScoped<MenuService>();
    services.AddHostedService<PollingService>();
    services.AddHostedService<AdminUsersInitService>();
});

await builder.Build().RunAsync();

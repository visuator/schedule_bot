using Microsoft.Extensions.Options;
using schedule_bot.Configuration;

namespace schedule_bot.Services;

public class AdminUsersInitService(IOptions<AdminConfiguration> adminConfiguration, IServiceScopeFactory serviceScopeFactory) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var scope = serviceScopeFactory.CreateAsyncScope();
        var userService = scope.ServiceProvider.GetRequiredService<IUserRepository>();
        foreach (var id in adminConfiguration.Value.Ids)
        {
            var userId = long.Parse(id);
            userService.GetOrCreateDefault(new CreateDefaultUser(userId, true));
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}

using System.ComponentModel.DataAnnotations;

namespace schedule_bot.Configuration;

public class BotConfiguration
{
    [Required] public string BotToken { get; set; } = default!;
}
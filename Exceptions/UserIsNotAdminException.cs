namespace schedule_bot.Common;

public class UserIsNotAdminException(long userId) : Exception(Resources.UserIsNotAdmin)
{
    public long UserId { get; } = userId;
}

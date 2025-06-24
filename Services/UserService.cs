using System.Text.Json;
using System.Text.Json.Serialization;
using LiteDB;
using schedule_bot.Entities;
using schedule_bot.Menus;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace schedule_bot.Services;

public record CreateDefaultUser(long UserId, bool IsAdmin);
public interface IUserRepository
{
    User GetOrCreateDefault(CreateDefaultUser dto);
}
public class UserRepository(LiteDatabase db) : IUserRepository
{
    private readonly Lock _lock = new();
    private readonly ILiteCollection<User> _users = db.GetCollection<User>();

    public User GetOrCreateDefault(CreateDefaultUser dto)
    {
        if (!db.BeginTrans())
            throw new Exception();
        lock (_lock)
        {
            try
            {
                var (userId, isAdmin) = dto;
                var user = _users.FindById(userId);
                if (user is null)
                {
                    user = new User()
                    {
                        IsAdmin = isAdmin,
                        Id = userId,
                        Settings = new()
                        {
                            BeginningEnabled = true,
                            DeadlineEnabled = true
                        },
                        Tasks = [],
                        MenuJson = "[]"
                    };
                    _users.Insert(user);
                }
                db.Commit();
                return user;
            }
            catch
            {
                db.Rollback();
                throw;
            }
        }
    }
}

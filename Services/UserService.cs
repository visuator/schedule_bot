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
    void ChangeMenu(long userId, IMenu menu);
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

    public void ChangeMenu(long userId, IMenu menu)
    {
        if (!db.BeginTrans())
            throw new Exception();
        lock (_lock)
        {
            try
            {
                var user = _users.FindById(userId);
                //todo: refactor this
                var snapshots = JsonSerializer.Deserialize<List<MenuSnapshot>>(user.MenuJson, MenuSnapshot.JsonSerializerOptions);
                ArgumentNullException.ThrowIfNull(snapshots);
                var dto = menu.CreateSnapshot();
                snapshots.RemoveAll(x => x.TypeName == dto.TypeName);
                snapshots.Add(dto);
                user.MenuJson = JsonSerializer.Serialize(snapshots, MenuSnapshot.JsonSerializerOptions);
                _users.Update(user);
                db.Commit();
            }
            catch
            {
                db.Rollback();
                throw;
            }
        }
    }
}

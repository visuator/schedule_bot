using schedule_bot.Menus.Abstract;
using schedule_bot.Services;

namespace schedule_bot.Menus.Impl;

public class MenuStorage(IUserRepository repository, IMenuSerializer serializer)
{
    public void Switch(long userId, IMenu menu)
    {
        var user = repository.GetOrCreateDefault(new(userId, false));
        var currentMenus = serializer.Deserialize(user.MenuString);
        var serialized = serializer.Serialize(
            currentMenus
                .Where(x => x.Name != menu.Name)
                .Append(menu)
        );
        repository.Upsert(new(userId, serialized));
    }

    public IMenu[] GetAll(long userId)
    {
        var user = repository.GetOrCreateDefault(new(userId, false));
        var currentMenus = serializer.Deserialize(user.MenuString);
        return currentMenus;
    }
}

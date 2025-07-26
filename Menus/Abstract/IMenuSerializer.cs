namespace schedule_bot.Menus.Abstract;

public interface IMenuSerializer
{
    IMenu[] Deserialize(string data);
    string Serialize(IEnumerable<IMenu> instances);
}

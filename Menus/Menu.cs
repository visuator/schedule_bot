using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using MediatR;
using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public interface IMenuRouter
{
    Task Route(RequestContext context);
}
public class MenuRouter(IMediator mediator, IMenuDeserializer menuDeserializer) : IMenuRouter
{
    private static readonly Dictionary<string, Func<RequestContext, IRequest>> _routes = new()
    {
        [@"DatePickerMenu.\d+"] = context => new DatePickerCommand(context),
        ["StartMenu.AddTask"] = context => new AddTaskCommand(context),
        ["StartMenu.SetupNotifications"] = context => new SetupNotificationsCommand(context),
        ["StartMenu.\ud83d\udcc6 Добавить выходные дни"] = context => new AddVacationCommand(context)
    };
    public Task Route(RequestContext context)
    {
        var menu = menuDeserializer.Deserialize(context.User.MenuJson);
        var data = context.Message?.Text ?? context.CallbackQuery?.Data;
        ArgumentNullException.ThrowIfNull(data);

        var key = $"{menu.Name}.{data}";

        IRequest? request = null;
        if (_routes.TryGetValue(key, out var route))
            request = route(context);
        else
        {
            foreach (var (k, v) in _routes)
            {
                var pattern = new Regex(key);
                if (!pattern.IsMatch(key))
                    continue;
                request = v(context);
                break;
            }
        }
        ArgumentNullException.ThrowIfNull(request);
        return mediator.Send(request);
    }
}
public interface IMenuDeserializer
{
    IMenu Deserialize(string json);
}
public class MenuDeserializer : IMenuDeserializer
{
    private static readonly Dictionary<string, Func<JsonObject, IMenu>> _factories = new()
    {
        ["StartMenu"] = json => new StartMenu(json),
        ["DatePickerMenu"] = data => new DatePickerMenu(data)
    };

    public IMenu Deserialize(string json)
    {
        var obj = JsonNode.Parse(json) as JsonObject;
        ArgumentNullException.ThrowIfNull(obj);
        return _factories[obj["Name"]!.GetValue<string>()](obj);
    }
}

public interface IMenu
{
    string Name { get; }
    ReplyMarkup GenerateKeyboard();
    string ToJsonString();
}
public abstract class Menu<TButton> : IMenu
{
    private readonly List<Menu<TButton>> _children = [];

    protected List<TButton[]> Rows { get; } = [];
    protected Menu()
    {
        Name = this.GetType().Name;
    }
    public Menu(string name)
    {
        Name = name;
    }

    public void Append(Menu<TButton> menu)
    {
        _children.Add(menu);
        Rows.AddRange(menu.Rows);
    }
    public void Append(params TButton[] row)
    {
        Rows.Add(row);
    }

    public string Name { get; }
    public abstract ReplyMarkup GenerateKeyboard();
    public abstract string ToJsonString();
}
public abstract class ReplyMenu : Menu<KeyboardButton>
{
    protected void AppendRow(params string[] texts)
    {
        Rows.Add(texts
            .Select(x => new KeyboardButton(x))
            .ToArray()
        );
    }
    protected JsonArray SerializeRows() => new([..Rows.Select(x => new JsonArray([..x.Select(x => x.Text)]))]);
    protected void DeserializeRows(JsonArray json)
    {
        foreach (JsonArray? row in json)
        {
            ArgumentNullException.ThrowIfNull(row);
            Rows.Add([..row.GetValues<string>()]);
        }
    }
    public override ReplyMarkup GenerateKeyboard() => new ReplyKeyboardMarkup(Rows);
}
public abstract class InlineMenu : Menu<InlineKeyboardButton>
{
    protected static InlineKeyboardButton NextPage { get; } = new("\u2192") { CallbackData = "next_page" };
    protected JsonArray SerializeRows() => new([..Rows.Select(x => new JsonArray([..x.Select(x => x.CallbackData)]))]);
    public override ReplyMarkup GenerateKeyboard() => new InlineKeyboardMarkup(Rows);
}

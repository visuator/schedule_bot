using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using LiteDB;
using MediatR;
using schedule_bot.Commands;
using schedule_bot.Entities;
using Telegram.Bot.Types.ReplyMarkups;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace schedule_bot.Menus;

public class MenuService(IMediator mediator, LiteDatabase db, MenuFactory factory)
{
    private readonly Lock _lock = new();
    private readonly ILiteCollection<User> _users = db.GetCollection<User>();
    public async Task Route(RequestContext context)
    {
        var menus = factory.Restore(context.User.Snapshots);
        foreach (var menu in menus)
        {
            if (await menu.TryRoute(mediator, context))
                return;
        }
    }
    public void Update(long userId, IMenu menu)
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
public class MenuFactory
{
    public IMenu[] Restore(IEnumerable<MenuSnapshot> snapshots) => snapshots.Select(Restore).ToArray();
    private IMenu Restore(MenuSnapshot snapshot) => snapshot switch
    {
        StartMenuSnapshot startMenuSnapshot => new StartMenu(startMenuSnapshot, this),
        TestComposeMenuSnapshot testComposeMenuSnapshot => new TestComposeMenu(testComposeMenuSnapshot, this),
        AdminMenuSnapshot adminMenuSnapshot => new AdminMenu(adminMenuSnapshot),
        StudentMenuSnapshot studentMenuSnapshot => new StudentMenu(studentMenuSnapshot),
        DatePickerMenuSnapshot datePickerMenuSnapshot => new DatePickerMenu(datePickerMenuSnapshot),
        SubjectMenuSnapshot subjectMenuSnapshot => new SubjectMenu(subjectMenuSnapshot),
        _ => throw new ArgumentOutOfRangeException(snapshot.GetType().Name)
    };
    public StartMenu CreateStartMenu(bool isAdmin) => new(isAdmin, this);
    public DatePickerMenu CreateDatePickerMenu(DateTime date) => new(date);
    public StudentMenu CreateStudentMenu() => new();
    public AdminMenu CreateAdminMenu() => new();
    public TestComposeMenu CreateTestComposeMenu(DateTime from, DateTime to) => new(from, to, this);
    public SubjectMenu CreateSubjectMenu(IEnumerable<string> subjects) => new(subjects);
}
public abstract class MenuSnapshot
{
    public string TypeName => GetType().Name;

    static MenuSnapshot()
    {
        JsonSerializerOptions = new();
        JsonSerializerOptions.Converters.Add(new MenuSnapshotConverter());
    }

    [JsonIgnore]
    public static JsonSerializerOptions JsonSerializerOptions { get; }
    private class MenuSnapshotConverter : JsonConverter<MenuSnapshot>
    {
        public override MenuSnapshot Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.PropertyName)
            {
                throw new JsonException();
            }

            string? propertyName = reader.GetString();
            if (propertyName != nameof(TypeName))
            {
                throw new JsonException();
            }

            reader.Read();
            if (reader.TokenType != JsonTokenType.String)
            {
                throw new JsonException();
            }

            var typeName = reader.GetString();
            MenuSnapshot snapshot = typeName switch
            {
                nameof(StartMenuSnapshot) => new StartMenuSnapshot(),
                nameof(TestComposeMenuSnapshot) => new TestComposeMenuSnapshot(),
                nameof(StudentMenuSnapshot) => new StudentMenuSnapshot(),
                nameof(DatePickerMenuSnapshot) => new DatePickerMenuSnapshot(),
                _ => throw new JsonException()
            };

            void UpdateField(string? name, object? rawValue)
            {
                ArgumentNullException.ThrowIfNull(name);
                ArgumentNullException.ThrowIfNull(rawValue);

                var type = snapshot.GetType();
                var property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
                ArgumentNullException.ThrowIfNull(property);

                property.SetValue(snapshot, Convert.ChangeType(rawValue, property.PropertyType));
            }

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndObject:
                        return snapshot;
                    case JsonTokenType.PropertyName:
                        propertyName = reader.GetString();
                        reader.Read();
                        UpdateField(propertyName, reader.GetString());
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(propertyName);
                }
            }

            return snapshot;
        }

        public override void Write(Utf8JsonWriter writer, MenuSnapshot value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WriteString(nameof(TypeName), value.TypeName);
            switch (value)
            {
                case StartMenuSnapshot startMenuSnapshot:
                    writer.WriteString(nameof(StartMenuSnapshot.IsAdmin), startMenuSnapshot.IsAdmin.ToString());
                    break;
                case TestComposeMenuSnapshot testComposeMenuSnapshot:
                    writer.WriteString(nameof(TestComposeMenuSnapshot.From), testComposeMenuSnapshot.From.ToString("O"));
                    writer.WriteString(nameof(TestComposeMenuSnapshot.To), testComposeMenuSnapshot.To.ToString("O"));
                    break;
                case DatePickerMenuSnapshot datePickerMenuSnapshot:
                    writer.WriteString(nameof(DatePickerMenuSnapshot.Date), datePickerMenuSnapshot.Date.ToString("O"));
                    break;
                case AdminMenuSnapshot:
                case StudentMenuSnapshot:
                    break;
            }

            writer.WriteEndObject();
        }
    }
}
public interface IMenu
{
    Task<bool> TryRoute(IMediator mediator, RequestContext context);
    MenuSnapshot CreateSnapshot();
}
public abstract class Menu<TButton> : IMenu
{
    protected List<Menu<TButton>> Children { get; } = [];
    protected List<TButton[]> Rows { get; } = [];
    protected Dictionary<string, Func<RequestContext, IRequest>> Routes { get; } = [];
    public abstract ReplyMarkup CreateMarkup();
    public abstract MenuSnapshot CreateSnapshot();
    public async Task<bool> TryRoute(IMediator mediator, RequestContext context)
    {
        var data = context.Message?.Text ?? context.CallbackQuery?.Data;
        ArgumentNullException.ThrowIfNull(data);

        IRequest? request = null;
        if (Routes.TryGetValue(data, out var route))
            request = route(context);
        else
        {
            foreach (var (k, v) in Routes)
            {
                var pattern = new Regex(k);
                if (!pattern.IsMatch(data))
                    continue;
                request = v(context);
                break;
            }
        }
        if (request is null)
            return false;
        await mediator.Send(request);
        return true;
    }
}
public abstract class ReplyMenu : Menu<KeyboardButton>
{
    protected void Append(ReplyMenu menu)
    {
        Children.Add(menu);
        Rows.AddRange(menu.Rows);
        foreach (var (k, v) in menu.Routes)
        {
            Routes.Add(k, v);
        }
    }
    protected void Append(params string[] texts)
    {
        Rows.Add(texts
            .Select(x => new KeyboardButton(x))
            .ToArray()
        );
    }
    public override ReplyMarkup CreateMarkup() => new ReplyKeyboardMarkup(Rows);
}
public abstract class InlineMenu : Menu<InlineKeyboardButton>
{
    protected static InlineKeyboardButton NextPage { get; } = new("\u2192") { CallbackData = "next_page" };
    protected void Append(InlineMenu menu)
    {
        Children.Add(menu);
        foreach (var row in menu.Rows)
        {
            if (Rows.FirstOrDefault(x => x.Length == 1 && x[0] == NextPage) is { } found && row.Length == 1 && row[0] == NextPage)
                continue;
            Rows.Add(row);
        }
        if (Rows.RemoveAll(x => x.Length == 1 && x[0] == NextPage) == 1)
            Rows.Add([NextPage]);
        foreach (var (k, v) in menu.Routes)
        {
            Routes[k] = v;
        }
    }
    public override ReplyMarkup CreateMarkup() => new InlineKeyboardMarkup(Rows);
}

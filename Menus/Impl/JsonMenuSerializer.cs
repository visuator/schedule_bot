using System.Text.Json;
using System.Text.Json.Serialization;
using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus.Impl;

public class JsonMenuSerializer : IMenuSerializer
{
    private readonly JsonSerializerOptions _serializerOptions;

    public JsonMenuSerializer(MenuFactory factory)
    {
        _serializerOptions = new()
        {
            Converters = { new MenuConverter(factory) }
        };
    }

    public IMenu[] Deserialize(string data)
    {
        var array = JsonSerializer.Deserialize<IMenu[]>(data, _serializerOptions);
        ArgumentNullException.ThrowIfNull(array);
        return array;
    }

    public string Serialize(IEnumerable<IMenu> instances) => JsonSerializer.Serialize(instances, _serializerOptions);

    private class MenuConverter(MenuFactory factory) : JsonConverter<IMenu>
    {
        public override IMenu? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.StartObject)
                throw new JsonException();
            reader.Read();
            ConsumeProperty(ref reader, "Name");
            var name = reader.GetString();
            ArgumentException.ThrowIfNullOrEmpty(name);
            reader.Read();

            IMenu? menu;
            switch (name)
            {
                case nameof(AdminMenu):
                    menu = factory.CreateAdminMenu();
                    break;
                case nameof(StudentMenu):
                    menu = factory.CreateStudentMenu();
                    break;
                case nameof(StartMenu):
                    ConsumeProperty(ref reader, "IsAdmin");
                    var isAdmin = reader.GetBoolean();
                    menu = factory.CreateStartMenu(isAdmin);
                    break;
                case nameof(DatePickerMenu):
                    ConsumeProperty(ref reader, "Date");
                    var date = reader.GetDateTime();
                    menu = factory.CreateDatePickerMenu(date);
                    break;
                case nameof(SubjectMenu):
                    ConsumeProperty(ref reader, "Subjects");
                    List<string> subjects = [];
                    if (reader.TokenType != JsonTokenType.StartArray)
                        throw new JsonException();
                    while (reader.Read() && reader.TokenType != JsonTokenType.EndArray)
                    {
                        var value = reader.GetString();
                        ArgumentException.ThrowIfNullOrEmpty(value);
                        subjects.Add(value);
                    }
                    if (!reader.Read())
                        throw new JsonException();
                    menu = factory.CreateSubjectMenu([.. subjects]);
                    break;
                default:
                    menu = null;
                    break;
            }
            reader.Read();
            if (reader.TokenType != JsonTokenType.EndObject)
                throw new JsonException();
            return menu;
        }

        public override void Write(Utf8JsonWriter writer, IMenu value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();
            writer.WritePropertyName("Name");
            writer.WriteStringValue(value.Name);
            switch (value)
            {
                case AdminMenu:
                    break;
                case StudentMenu:
                    break;
                case StartMenu menu:
                    writer.WritePropertyName("IsAdmin");
                    writer.WriteBooleanValue(menu.IsAdmin);
                    break;
                case DatePickerMenu menu:
                    writer.WritePropertyName("Date");
                    writer.WriteStringValue(menu.Date);
                    break;
                case SubjectMenu menu:
                    writer.WritePropertyName("Subjects");
                    writer.WriteStartArray();
                    foreach (var subject in menu.Subjects)
                    {
                        writer.WriteStringValue(subject);
                    }
                    writer.WriteEndArray();
                    break;
                default:
                    throw new JsonException();
            }
            writer.WriteEndObject();
        }

        private static void ConsumeProperty(ref Utf8JsonReader reader, string name)
        {
            if (reader.TokenType != JsonTokenType.PropertyName)
                throw new JsonException();
            var property = reader.GetString();
            ArgumentException.ThrowIfNullOrEmpty(property);
            if (property != name)
                throw new JsonException();
            reader.Read();
        }
    }
}
using System.Text.Json.Nodes;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class DatePickerMenu : InlineMenu
{
    private readonly DateTime _from;
    public DatePickerMenu(DateTime from)
    {
        _from = from;
        Configure(_from);
    }
    public DatePickerMenu(JsonObject json)
    {
        _from = json["From"]!.GetValue<DateTime>();
        Configure(_from);
    }

    private void Configure(DateTime from)
    {
        var daysCount = DateTime.DaysInMonth(from.Year, from.Month);
        var remainedDayNumbers = Enumerable.Range(from.Day, daysCount - from.Day)
            .Select(x => $"{x}")
            .ToHashSet();

        Rows.AddRange(remainedDayNumbers
            .Select(x => new InlineKeyboardButton(x) { CallbackData = x })
            .Chunk(3)
            .Append([NextPage])
        );
    }

    public override string ToJsonString()
    {
        var root = new JsonObject()
        {
            ["Name"] = Name,
            ["From"] = _from,
            ["Buttons"] = SerializeRows()
        };
        return root.ToJsonString();
    }
}

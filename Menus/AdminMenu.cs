using System.Text.Json.Nodes;

namespace schedule_bot.Menus;

public class AdminMenu : ReplyMenu
{
    public AdminMenu()
    {
        AppendRow(Resources.AddVacation);
    }

    public override string ToJsonString()
    {
        var root = new JsonObject()
        {
            ["Name"] = Name,
            ["Buttons"] = SerializeRows()
        };
        return root.ToJsonString();
    }
}

using System.Text.Json.Nodes;

namespace schedule_bot.Menus;

public class StudentMenu : ReplyMenu
{
    public StudentMenu()
    {
        AppendRow(Resources.TodayScheduling, Resources.TomorrowScheduling, Resources.FullScheduling);
        AppendRow(Resources.ShowList, Resources.AddTask, Resources.UpdateTaskStatus);
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

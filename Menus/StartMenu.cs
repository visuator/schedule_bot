using System.Text.Json.Nodes;

namespace schedule_bot.Menus;

public class StartMenu : ReplyMenu
{
    private readonly bool _isAdmin;
    public StartMenu(bool isAdmin)
    {
        _isAdmin = isAdmin;
        Configure(_isAdmin);
    }
    public StartMenu(JsonObject json)
    {
        _isAdmin = json["IsAdmin"]!.GetValue<bool>();
        Configure(_isAdmin);
    }

    private void Configure(bool isAdmin)
    {
        Append(new StudentMenu());
        if (isAdmin)
            Append(new AdminMenu());
    }

    public override string ToJsonString()
    {
        var root = new JsonObject()
        {
            ["Name"] = Name,
            ["IsAdmin"] = _isAdmin,
            ["Buttons"] = SerializeRows()
        };
        return root.ToJsonString();
    }
}

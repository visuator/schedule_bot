using schedule_bot.Commands;
using Telegram.Bot.Types.ReplyMarkups;

namespace schedule_bot.Menus;

public class SubjectMenuSnapshot : MenuSnapshot
{
    public IEnumerable<string> Subjects { get; set; } = [];
}
public class SubjectMenu : InlineMenu
{
    public SubjectMenu(IEnumerable<string> subjects)
    {
        Rows.AddRange([.. subjects.Select(x => new InlineKeyboardButton(x, x)).Chunk(3)]);
        Routes[@"\w+"] = context => new AddTaskCommand(context); 
    }

    public SubjectMenu(SubjectMenuSnapshot snapshot) : this(snapshot.Subjects) { }

    public override MenuSnapshot CreateSnapshot() => new SubjectMenuSnapshot();
}
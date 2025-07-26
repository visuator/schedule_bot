using schedule_bot.Commands;
using schedule_bot.Menus.Abstract;

namespace schedule_bot.Menus;

public class SubjectMenu : InlineMenu
{
    public string[] Subjects { get; }

    public SubjectMenu(string[] subjects)
    {
        Subjects = subjects;

        Rows.AddRange([.. subjects.Select(x => new MediatRInlineMenuButton(x, x, context => new AddTaskCommand(context))).Chunk(3)]);
    }
}
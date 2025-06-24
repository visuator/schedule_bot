namespace schedule_bot.Menus;

public class StudentMenuSnapshot : MenuSnapshot;
public class StudentMenu : ReplyMenu
{
    public StudentMenu()
    {
        Append(Resources.TodayScheduling, Resources.TomorrowScheduling, Resources.FullScheduling);
        Append(Resources.ShowList, Resources.AddTask, Resources.UpdateTaskStatus);
    }
    public StudentMenu(StudentMenuSnapshot snapshot) : this() { }
    public override MenuSnapshot CreateSnapshot() => new StudentMenuSnapshot();
}

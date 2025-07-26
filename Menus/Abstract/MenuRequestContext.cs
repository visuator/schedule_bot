using schedule_bot.Commands;

namespace schedule_bot.Menus.Abstract;

public record MenuRequestContext(IMenu Menu, RequestContext BaseContext);

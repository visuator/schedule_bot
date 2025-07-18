﻿using MediatR;
using schedule_bot.Extensions;
using schedule_bot.Menus;
using Telegram.Bot;

namespace schedule_bot.Commands;

public record AddVacationCommand(RequestContext Context) : IRequest
{
    public class Handler(ITelegramBotClient client, MenuFactory factory, MenuService menuService) : IRequestHandler<AddVacationCommand>
    {
        public async Task Handle(AddVacationCommand request, CancellationToken token)
        {
            ArgumentNullException.ThrowIfNull(request.Context.Message);
            var menu = factory.CreateTestComposeMenu(DateTime.Today, DateTime.Today.AddDays(1));
            menuService.Update(request.Context.User.Id, menu);
            await client.SendMessage(
                chatId: request.Context.Message.GetUserId(),
                text: "Выбери начальную дату",
                replyMarkup: menu.CreateMarkup(),
                cancellationToken: token
            );
        }
    }
}

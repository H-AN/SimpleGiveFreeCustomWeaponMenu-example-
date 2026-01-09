using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Commands;
using SwiftlyS2.Shared.Players;

namespace HanFreeWeapon;

public class Commands
{
    private readonly ILogger<Commands> _logger;
    private readonly ISwiftlyCore _core;
    private readonly Menu _menus;
    public Commands(ISwiftlyCore core, ILogger<Commands> logger,
        Menu menu)
    {
        _core = core;
        _logger = logger;
        _menus = menu;
    }

    public void commands()
    {
        _core.Command.RegisterCommand("sw_weapon", OpenWeaponMenu, true);
    }

    public void OpenWeaponMenu(ICommandContext context)
    {
        var player = context.Sender;
        if (player == null || !player.IsValid)
            return;

        var Controller = player.Controller;
        if (Controller == null || !Controller.IsValid)
            return;

        if (!Controller.PawnIsAlive)
        {
            player.SendMessage(MessageType.Chat, $"必须活着才能使用");
            return;
        }


        _menus.OpenFreeMenu(player);

    }
}
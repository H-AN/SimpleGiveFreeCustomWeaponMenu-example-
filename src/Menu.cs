using System.Drawing;
using System.Reflection.Emit;
using System.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mono.Cecil.Cil;
using SwiftlyS2.Core.Menus.OptionsBase;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Menus;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.Plugins;
using SwiftlyS2.Shared.SteamAPI;

namespace HanFreeWeapon;

public class Menu
{
    private readonly ILogger<Menu> _logger;
    private readonly ISwiftlyCore _core;
    private readonly MenuHelper _menuhelper;
    private readonly IOptionsMonitor<FreeWeaponConfig> _config;

    public bool[] MenuCD { get; set; } = new bool[65];
    public Menu(ISwiftlyCore core, ILogger<Menu> logger,
        MenuHelper menuHelper,
        IOptionsMonitor<FreeWeaponConfig> config)
    {
        _core = core;
        _logger = logger;
        _menuhelper = menuHelper;
        _config = config;
    }
    
    public IMenuAPI OpenFreeMenu(IPlayer player)
    {
        var main = _core.MenusAPI.CreateBuilder();
        IMenuAPI menu = _menuhelper.CreateMenu($"[华仔]免费武器菜单");

        // 顶部滚动文字
        menu.AddOption(new TextMenuOption(HtmlGradient.GenerateGradientText(
            $"选择你想获取的武器",
            Color.Red, Color.LightBlue, Color.Red),
            updateIntervalMs: 500, pauseIntervalMs: 100)
        {
            TextStyle = MenuOptionTextStyle.ScrollLeftLoop
        });

        var List = _config.CurrentValue.CommandList;
        if (List != null && List.Count > 0)
        {
            foreach (var Cfg in List)
            {

                var pawn = player.PlayerPawn;
                if(pawn == null || !pawn.IsValid)
                    continue;

                if(pawn.TeamNum != 3)
                    continue;


                string buttonText = $"{Cfg.WeaponName}";

                var turretButton = new ButtonMenuOption(buttonText)
                {
                    TextStyle = MenuOptionTextStyle.ScrollLeftLoop,
                    CloseAfterClick = false
                };
                turretButton.Tag = "extend";

                turretButton.Click += async (_, args) =>
                {
                    var clicker = args.Player;
                    _core.Scheduler.NextTick(() =>
                    {
                        if (!clicker.IsValid)
                            return;

                        if (MenuCD[player.PlayerID])
                        {
                            player.SendMessage(MessageType.Chat, "使用有CD,不要乱刷武器!!");
                            return;
                        }

                        player.ExecuteCommand($"{Cfg.WeaponCommand}");
                        MenuCD[player.PlayerID] = true;
                        _core.Scheduler.DelayBySeconds(1.0f, () =>{ MenuCD[player.PlayerID] = false; });
                    });
                };

                menu.AddOption(turretButton);
            }
        }

        _core.MenusAPI.OpenMenuForPlayer(player, menu);
        return menu;
    }


}

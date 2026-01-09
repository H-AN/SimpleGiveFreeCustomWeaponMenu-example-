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

namespace HanWeaponMenu;

public class Menu
{
    private readonly ILogger<Menu> _logger;
    private readonly ISwiftlyCore _core;
    private readonly MenuHelper _menuhelper;
    private readonly IOptionsMonitor<WeaponMenu> _config;

    public Dictionary<int, bool> MenuCD { get; set; } = new Dictionary<int, bool>();

    public Menu(ISwiftlyCore core, ILogger<Menu> logger,
        MenuHelper menuHelper,
        IOptionsMonitor<WeaponMenu> config)
    {
        _core = core;
        _logger = logger;
        _menuhelper = menuHelper;
        _config = config;
    }
    
    public IMenuAPI OpenFreeMenu(IPlayer player)
    {
        var main = _core.MenusAPI.CreateBuilder();
        IMenuAPI menu = _menuhelper.CreateMenu($"{_core.Translation.GetPlayerLocalizer(player)["MenuTitle"]}");

        // 顶部滚动文字
        menu.AddOption(new TextMenuOption(HtmlGradient.GenerateGradientText(
            $"{_core.Translation.GetPlayerLocalizer(player)["MenuSelectWeapons"]}",
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

                int team = pawn.TeamNum;
                string cfgTeam = Cfg.Team?.ToUpper() ?? "ALL";

                
                if (cfgTeam == "CT" && team != 3)
                    continue;

                if (cfgTeam == "T" && team != 2)
                    continue;

                if (cfgTeam != "CT" && cfgTeam != "T" && cfgTeam != "ALL")
                    continue;
                
                string buttonText = $"{Cfg.WeaponName}|{Cfg.Price}$";

                var Button = new ButtonMenuOption(buttonText)
                {
                    TextStyle = MenuOptionTextStyle.ScrollLeftLoop,
                    CloseAfterClick = false
                };
                Button.Tag = "extend";

                Button.Click += async (_, args) =>
                {
                    var clicker = args.Player;

                    _core.Scheduler.NextTick(() =>
                    {
                        if (!clicker.IsValid)
                            return;

                        
                        if (MenuCD.TryGetValue(clicker.PlayerID, out bool cd) && cd)
                        {
                            clicker.SendMessage(MessageType.Chat, $"{_core.Translation.GetPlayerLocalizer(player)["MenuCD"]}");
                            return;
                        }

                        
                        var money = clicker.Controller.InGameMoneyServices;
                        if (money == null || !money.IsValid)
                            return;

                        int current = money.Account;

                        
                        if (current < Cfg.Price)
                        {
                            clicker.SendMessage(MessageType.Chat, $"{_core.Translation.GetPlayerLocalizer(player)["NeedMoney", $"{Cfg.Price}$"]}"); //你的金钱不足，需要 {Cfg.Price}$!
                            return;
                        }

                        
                        current -= Cfg.Price;
                        money.Account = current;
                        clicker.Controller.InGameMoneyServicesUpdated();

                        
                        if (!string.IsNullOrWhiteSpace(Cfg.WeaponCommand))
                            clicker.ExecuteCommand(Cfg.WeaponCommand);

                        
                        MenuCD[clicker.PlayerID] = true;
                        _core.Scheduler.DelayBySeconds(1.0f, () =>
                        {
                            MenuCD[clicker.PlayerID] = false;
                        });
                    });
                };

                menu.AddOption(Button);
            }
        }

        _core.MenusAPI.OpenMenuForPlayer(player, menu);
        return menu;
    }


}

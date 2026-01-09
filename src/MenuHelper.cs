using System.Reflection.Emit;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Mono.Cecil.Cil;
using SwiftlyS2.Core.Menus.OptionsBase;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Menus;
using SwiftlyS2.Shared.Players;
using SwiftlyS2.Shared.Plugins;
using System.Drawing;

namespace HanFreeWeapon;

public class MenuHelper
{
    private readonly ILogger<MenuHelper> _logger;
    private readonly ISwiftlyCore _core;

    public MenuHelper(ISwiftlyCore core, ILogger<MenuHelper> logger)
    {
        _core = core;
        _logger = logger;
    }

    public IMenuAPI CreateMenu(string title)
    {
        MenuConfiguration configuration = new()
        {
            Title = HtmlGradient.GenerateGradientText(title, Color.LightGreen),
            FreezePlayer = false,
            MaxVisibleItems = 5,
            PlaySound = true,
            AutoIncreaseVisibleItems = false,
            HideFooter = true
        };

        MenuKeybindOverrides keys = new MenuKeybindOverrides()
        {
            Move = KeyBind.S,
            MoveBack = KeyBind.W,
            Exit = KeyBind.Shift,
            Select = KeyBind.E
        };

        IMenuAPI menu = _core.MenusAPI.CreateMenu(
            configuration,
            keybindOverrides: keys,
            optionScrollStyle: MenuOptionScrollStyle.WaitingCenter
            );
        
        menu.DefaultComment = HtmlGradient.GenerateGradientText("[W/S]", Color.Crimson) + HtmlGradient.GenerateGradientText($"移动", Color.White)
            + HtmlGradient.GenerateGradientText("[E]", Color.Crimson) + HtmlGradient.GenerateGradientText($"确认", Color.White)
            + HtmlGradient.GenerateGradientText("[SHIFT]", Color.Crimson) + HtmlGradient.GenerateGradientText($"取消", Color.White)
            ;
        
        return menu;
    }

}
    

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;
using Microsoft.Extensions.Logging;

namespace HanWeaponMenu;

[PluginMetadata(
    Id = "HanWeaponMenu",
    Version = "1.1",
    Name = "H-AN 武器菜单/H-AN Weapons menu",
    Author = "H-AN",
    Description = "简单武器系统菜单,支持价格与队伍/Simple weapon system menu, supports price and team")]

public partial class HanTurretS2(ISwiftlyCore core) : BasePlugin(core)
{
    private ServiceProvider? ServiceProvider { get; set; }
    private Commands _Commands = null!;
    private IOptionsMonitor<WeaponMenu> _CFGMonitor = null!;
    public override void Load(bool hotReload)
    {
        Core.Configuration.InitializeJsonWithModel<WeaponMenu>("WeaponMenuConfig.jsonc", "WeaponMenuCFG").Configure(builder =>
        {
            builder.AddJsonFile("WeaponMenuConfig.jsonc", false, true);
        });

        var collection = new ServiceCollection();
        collection.AddSwiftly(Core);

        collection
            .AddOptionsWithValidateOnStart<WeaponMenu>()
            .BindConfiguration("WeaponMenuCFG");

        collection.AddSingleton<Commands>();
        collection.AddSingleton<MenuHelper>();
        collection.AddSingleton<Menu>();

        ServiceProvider = collection.BuildServiceProvider();

        _Commands = ServiceProvider.GetRequiredService<Commands>();
       
        var Menus = ServiceProvider.GetRequiredService<Menu>();
        var MenusHelper = ServiceProvider.GetRequiredService<MenuHelper>();

        _CFGMonitor = ServiceProvider.GetRequiredService<IOptionsMonitor<WeaponMenu>>();

        _CFGMonitor.OnChange(newConfig =>
        {
            Core.Logger.LogInformation($"weapon menu hot update successful");
        });

        _Commands.commands();
    }

    public override void Unload()
    {
        ServiceProvider!.Dispose();
    }

}
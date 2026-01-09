
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using SwiftlyS2.Shared;
using SwiftlyS2.Shared.Plugins;
using Microsoft.Extensions.Logging;

namespace HanFreeWeapon;

[PluginMetadata(
    Id = "HanFreeWeaponMenu",
    Version = "1.0",
    Name = "H-AN 免费武器菜单/H-AN Free Weapons",
    Author = "H-AN",
    Description = "CS2 免费武器菜单/Free Weapons")]

public partial class HanTurretS2(ISwiftlyCore core) : BasePlugin(core)
{
    private ServiceProvider? ServiceProvider { get; set; }
    private Commands _Commands = null!;
    private IOptionsMonitor<FreeWeaponConfig> _CFGMonitor = null!;
    public override void Load(bool hotReload)
    {
        Core.Configuration.InitializeJsonWithModel<FreeWeaponConfig>("FreeWeaponConfig.jsonc", "FreeWeaponCFG").Configure(builder =>
        {
            builder.AddJsonFile("FreeWeaponConfig.jsonc", false, true);
        });

        var collection = new ServiceCollection();
        collection.AddSwiftly(Core);

        collection
            .AddOptionsWithValidateOnStart<FreeWeaponConfig>()
            .BindConfiguration("FreeWeaponCFG");

        collection.AddSingleton<Commands>();
        collection.AddSingleton<MenuHelper>();
        collection.AddSingleton<Menu>();

        ServiceProvider = collection.BuildServiceProvider();

        _Commands = ServiceProvider.GetRequiredService<Commands>();
       
        var Menus = ServiceProvider.GetRequiredService<Menu>();
        var MenusHelper = ServiceProvider.GetRequiredService<MenuHelper>();

        _CFGMonitor = ServiceProvider.GetRequiredService<IOptionsMonitor<FreeWeaponConfig>>();

        _CFGMonitor.OnChange(newConfig =>
        {
            Core.Logger.LogInformation($"免费武器菜单热更新成功");
        });

        _Commands.commands();
    }

    public override void Unload()
    {
        ServiceProvider!.Dispose();
    }

}
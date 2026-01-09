```
HanWeaponSystemS2 自定义武器插件的 武器菜单逻辑,用于武器发放使用例子 增加了队伍限定,价格限制, 配置文件中可以 定义价格与队伍可见
HanWeaponSystemS2 This custom weapon plugin's weapon menu logic, used as an example for weapon distribution, adds team-specific and price-limited restrictions.
Prices and team visibility can be defined in the configuration file.
```
配置文件示例/Configuration file example

```
{
  "WeaponMenuCFG": {
    "CommandList": [
      {
        "WeaponName": "AS50狙击步枪",
        "WeaponCommand": "sw_sb2YWrszaUeZkbfx",
        "Price": "4300",
        "Team": "CT"
      },
      {
        "WeaponName": "sg552-地狱犬",
        "WeaponCommand": "sw_i8mo0MCtSp6VfUPP",
        "Price": "2800",
        "Team": "T"
      },
      {
        "WeaponName": "圣诞毁灭者",
        "WeaponCommand": "sw_kYOuiOxgr0mSnCMv",
        "Price": "3500",
        "Team": "ALL"
      }
    ]
  }
}
```
---
此插件创建一个菜单,并将武器密码指令填入配置文件,填写 队伍可见(CT/T/ALL) 填写价格

当玩家输入!weapon后打开菜单,选择项目,之后服务器将自动为玩家发送隐藏指令,并让玩家获取自定义武器,扣除金钱等.

设置为CT队伍的武器只会出现在CT的菜单内,T同理,填写ALL的所有队伍可见

这只是一个简单的使用示例,你可以根据例子配置

---
This plugin creates a menu and populates the weapon code command into a configuration file. Enter "Visible to Team (CT/T/ALL)" and the price.

When a player types "!weapon", the menu opens, they select an item, and the server automatically sends the hidden command to the player, granting them a custom weapon and deducting money.

Weapons set for the CT team will only appear in the CT menu, and the same applies to the T team. Weapons set to "ALL" will be visible to all teams.

This is just a simple usage example; you can configure it according to this example.


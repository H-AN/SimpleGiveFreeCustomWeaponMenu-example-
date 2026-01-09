namespace HanWeaponMenu;

public class WeaponMenu
{
    public class AllCommands
    {
        public string WeaponName { get; set; } = string.Empty;
        public int Price { get; set; } = 0;
        public string Team { get; set; } = string.Empty;
        public string WeaponCommand { get; set; } = string.Empty;
    }
    public List<AllCommands> CommandList { get; set; } = new List<AllCommands>();
}
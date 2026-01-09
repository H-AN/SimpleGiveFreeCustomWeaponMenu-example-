namespace HanFreeWeapon;

public class FreeWeaponConfig
{
    public class AllCommands
    {
        public string WeaponName { get; set; } = string.Empty;
        public string WeaponCommand { get; set; } = string.Empty;
    }
    public List<AllCommands> CommandList { get; set; } = new List<AllCommands>();
}
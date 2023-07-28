namespace DoorCorrode
{
    using System.ComponentModel;
    using Exiled.API.Enums;
    using Exiled.API.Interfaces;

    public sealed class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
         
        public bool Debug { get; set; } = false;

        [Description("Keybind to toggle door corrosion - no SecondaryFirearm")]
        public HotkeyButton Hotkey { get; set; } = HotkeyButton.Medical;
        [Description("Cooldown before door corrosion can be triggered again")]
        public float Cooldown { get; set; } = 8f;
        [Description("How long door corrosion lasts")]
        public float Length { get; set; } = 3;
        [Description("How much Vigor door corrosion costs")]
        public float VigorCost { get; set; } = 0f;
        [Description("Whether or not a door has to be fully closed in order to be corroded")]
        public Boolean FullyClosed { get; set; } = false;
        [Description("The hint displayed on door corrosion cooldown")]
        public string CooldownHint { get; set; } = "<voffset=-7em><align=center><color=#634b4b><b>- DOOR CORROSION COOLDOWN -</b></color></align></voffset>";
        [Description("The hint displayed while corroding doors")]
        public string ActiveHint { get; set; } = "<voffset=-7em><align=center><color=#4a1318><b>- CURRENTLY CORRODING -</b></color></align></voffset>";
        [Description("Whether or not door corrosion works on gates")]
        public Boolean DoGates { get; set; } = true;
    }
}

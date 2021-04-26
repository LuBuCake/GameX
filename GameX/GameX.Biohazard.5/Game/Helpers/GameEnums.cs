﻿using System.ComponentModel;

namespace GameX.Game.Helpers
{
    public class GameEnums
    {
        public enum ItemGroup
        {
            [Description("Default")]
            Default = 0,

            [Description("Handguns")]
            Handgun = 1,

            [Description("Shotguns")]
            Shotgun = 2,

            [Description("Machine Guns")]
            MachineGun = 3,

            [Description("Rifles")]
            Rifle = 4,

            [Description("Magnums")]
            Magnum = 5,

            [Description("Launchers")]
            Launcher = 6,

            [Description("Melees")]
            Melee = 7,

            [Description("Explosives")]
            Explosive = 8,

            [Description("Ammunitions")]
            Ammunition = 9,

            [Description("Heals")]
            Heal = 10,

            [Description("Utility")]
            Utility = 11,

            [Description("Special")]
            Special = 12
        }
    }
}

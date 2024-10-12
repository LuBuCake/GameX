using GameX.Enum;
using GameX.Database.Type;
using System.Collections.Generic;

namespace GameX.Database.Content
{
    public static class ItemContent
    {
        public static List<Item> GetCollection()
        {
            Item Default = new Item
            {
                ID = 0,
                Group = ItemGroupEnum.Default,
                GroupIndex = 0,
                Name = "Nothing",
                Alias = "nothing",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 0 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item HandToHand = new Item
            {
                ID = 256,
                Group = ItemGroupEnum.Default,
                GroupIndex = 1,
                Name = "Hand To Hand",
                Alias = "handtohand",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item M92F = new Item
            {
                ID = 258,
                Group = ItemGroupEnum.Handgun,
                GroupIndex = 0,
                Name = "M92F (HG)",
                Alias = "m92f",
                Firepower = new[] { 150, 170, 190, 210, 230, 250 },
                ReloadSpeed = new[] { 1.70, 1.62, 1.53, 1.36 },
                Capacity = new[] { 10, 13, 16, 20, 25, 30, 33, 37, 40, 45, 50, 60, 70, 100 },
                Critical = new[] { 0, 1, 2, 3 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item HKP8 = new Item
            {
                ID = 272,
                Group = ItemGroupEnum.Handgun,
                GroupIndex = 1,
                Name = "H&K P8 (HG)",
                Alias = "h&kp8",
                Firepower = new[] { 140, 160, 180, 200, 220, 240, 260, 300 },
                ReloadSpeed = new[] { 1.53, 1.36, 1.19, 1.11, 1.02, 0.95 },
                Capacity = new[] { 9, 11, 13, 15, 17, 19, 21, 25 },
                Critical = new[] { 0 },
                Piercing = new[] { 0, 1, 2, 3 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item SIGP226 = new Item
            {
                ID = 273,
                Group = ItemGroupEnum.Handgun,
                GroupIndex = 2,
                Name = "SIG P226 (HG)",
                Alias = "sigp226",
                Firepower = new[] { 180, 220, 240, 260, 280, 300, 320, 340, 350, 370, 390, 410, 430, 480 },
                ReloadSpeed = new[] { 1.70, 1.62 },
                Capacity = new[] { 8, 10, 12, 13, 14, 16 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item M93R = new Item
            {
                ID = 286,
                Group = ItemGroupEnum.Handgun,
                GroupIndex = 3,
                Name = "M93R (HG)",
                Alias = "m93r",
                Firepower = new[] { 170, 190, 210, 230, 250, 270, 290, 310, 330, 350, 370, 400 },
                ReloadSpeed = new[] { 1.70, 1.62, 1.53, 1.36 },
                Capacity = new[] { 10, 12, 14, 16, 18, 20, 22, 24, 26, 30 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Samurai_Edge = new Item
            {
                ID = 297,
                Group = ItemGroupEnum.Handgun,
                GroupIndex = 4,
                Name = "Samurai Edge (HG)",
                Alias = "samuraiedge",
                Firepower = new[] { 400 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 15 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Samurai_Edge_DLC = new Item
            {
                ID = 274,
                Group = ItemGroupEnum.Handgun,
                GroupIndex = 5,
                Name = "Samurai Edge DLC (HG)",
                Alias = "samuraiedgedlc",
                Firepower = new[] { 340 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 30 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Px4 = new Item
            {
                ID = 287,
                Group = ItemGroupEnum.Handgun,
                GroupIndex = 6,
                Name = "Px4 (HG)",
                Alias = "px4",
                Firepower = new[] { 300 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 25 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Ithaca = new Item
            {
                ID = 260,
                Group = ItemGroupEnum.Shotgun,
                GroupIndex = 0,
                Name = "Ithaca (SG)",
                Alias = "ithaca",
                Firepower = new[] { 200, 230, 260, 300, 330, 360, 400 },
                ReloadSpeed = new[] { 3.00, 2.85, 2.70, 2.40 },
                Capacity = new[] { 6, 7, 8, 9, 10, 12, 13, 15, 16, 17, 18, 20, 22, 25 },
                Critical = new[] { 0, 1 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item M3 = new Item
            {
                ID = 278,
                Group = ItemGroupEnum.Shotgun,
                GroupIndex = 1,
                Name = "M3 (SG)",
                Alias = "m3",
                Firepower = new[] { 300, 320, 350, 370, 400, 420, 450, 480, 500, 550, 600, 650, 700, 900 },
                ReloadSpeed = new[] { 3.00, 2.70 },
                Capacity = new[] { 5, 6, 7, 8, 9, 10 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item JailBreaker = new Item
            {
                ID = 279,
                Group = ItemGroupEnum.Shotgun,
                GroupIndex = 2,
                Name = "Jail Breaker (SG)",
                Alias = "jailbreaker",
                Firepower = new[] { 180, 200, 220, 250, 300, 350 },
                ReloadSpeed = new[] { 2.52, 2.38, 2.24, 2.10, 1.96, 1.82 },
                Capacity = new[] { 5, 6, 7, 8, 9, 10, 11, 12, 13, 15 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0, 1, 2 },
                Scope = new[] { 0 }
            };

            Item Hydra = new Item
            {
                ID = 281,
                Group = ItemGroupEnum.Shotgun,
                GroupIndex = 3,
                Name = "Hydra (SG)",
                Alias = "hydra",
                Firepower = new[] { 280, 290, 310, 330, 350, 380, 400, 420, 440, 460, 500, 550 },
                ReloadSpeed = new[] { 3.67, 3.30 },
                Capacity = new[] { 4, 5, 6, 7, 8, 9, 10 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0, 1, 2 },
                Scope = new[] { 0 }
            };

            Item VZ61 = new Item
            {
                ID = 259,
                Group = ItemGroupEnum.MachineGun,
                GroupIndex = 0,
                Name = "VZ61 (MG)",
                Alias = "vz61",
                Firepower = new[] { 50, 60, 80, 100 },
                ReloadSpeed = new[] { 2.83, 2.69, 2.55, 2.27 },
                Capacity = new[] { 50, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300 },
                Critical = new[] { 0, 1, 2 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item AK74 = new Item
            {
                ID = 285,
                Group = ItemGroupEnum.MachineGun,
                GroupIndex = 1,
                Name = "AK74 (MG)",
                Alias = "ak74",
                Firepower = new[] { 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 220, 250 },
                ReloadSpeed = new[] { 2.83, 2.55 },
                Capacity = new[] { 30, 35, 40, 45, 50 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item MP5 = new Item
            {
                ID = 275,
                Group = ItemGroupEnum.MachineGun,
                GroupIndex = 2,
                Name = "H&K MP5 (MG)",
                Alias = "h&kmp5",
                Firepower = new[] { 60, 80, 90, 100, 120 },
                ReloadSpeed = new[] { 2.70, 2.55, 2.40, 2.10 },
                Capacity = new[] { 45, 55, 65, 80, 90, 100, 110, 120, 130, 150 },
                Critical = new[] { 0 },
                Piercing = new[] { 0, 1, 2 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item SIG556 = new Item
            {
                ID = 265,
                Group = ItemGroupEnum.MachineGun,
                GroupIndex = 3,
                Name = "SIG 556 (MG)",
                Alias = "sig556",
                Firepower = new[] { 80, 90, 100, 120, 130, 140, 150, 160, 180 },
                ReloadSpeed = new[] { 2.55, 2.41, 2.27, 1.98, 1.70, 1.42 },
                Capacity = new[] { 40, 45, 50, 55, 60, 65, 70, 80 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item S75 = new Item
            {
                ID = 261,
                Group = ItemGroupEnum.Rifle,
                GroupIndex = 0,
                Name = "S75 (RIF)",
                Alias = "s75",
                Firepower = new[] { 750, 800, 850, 900, 950, 1050, 1120, 1200, 1270, 1350, 1420, 1500, 1700, 2000 },
                ReloadSpeed = new[] { 3.67, 3.30 },
                Capacity = new[] { 6, 7, 8, 10, 12, 15, 17, 20, 22, 25, 40, 50 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Dragunov = new Item
            {
                ID = 288,
                Group = ItemGroupEnum.Rifle,
                GroupIndex = 1,
                Name = "Dragunov SVD (RIF)",
                Alias = "dragunov",
                Firepower = new[] { 650, 700, 750, 800, 850, 900, 950, 1000, 1100, 1300 },
                ReloadSpeed = new[] { 2.83, 2.69, 2.55, 2.41 },
                Capacity = new[] { 7, 9, 10, 12, 13, 15, 16, 18 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0, },
                Scope = new[] { 0 }
            };

            Item PSG1 = new Item
            {
                ID = 284,
                Group = ItemGroupEnum.Rifle,
                GroupIndex = 2,
                Name = "H&K PSG-1 (RIF)",
                Alias = "h&kpsg1",
                Firepower = new[] { 600, 650, 700, 750, 800, 900, 1000, 1200 },
                ReloadSpeed = new[] { 2.55, 2.41, 2.27, 2.13, 1.98, 1.70 },
                Capacity = new[] { 5, 6, 7, 9, 11, 15 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0, 1, 2 }
            };

            Item SEWM29 = new Item
            {
                ID = 267,
                Group = ItemGroupEnum.Magnum,
                GroupIndex = 0,
                Name = "S&W M29 (MAG)",
                Alias = "s&wm29",
                Firepower = new[] { 1500, 1700, 1900, 2100, 2400, 2700, 3200 },
                ReloadSpeed = new[] { 3.53, 3.36, 3.18, 2.83 },
                Capacity = new[] { 6, 7, 8, 9, 10, 11, 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0, 1 },
                Scope = new[] { 0 }
            };

            Item LHAWK = new Item
            {
                ID = 282,
                Group = ItemGroupEnum.Magnum,
                GroupIndex = 1,
                Name = "L. Hawk (MAG)",
                Alias = "lhawk",
                Firepower = new[] { 1400, 1600, 1800, 2000, 2300, 2600, 3000 },
                ReloadSpeed = new[] { 1.70, 1.62, 1.53, 1.45, 1.36 },
                Capacity = new[] { 5, 6, 7, 8 },
                Critical = new[] { 0 },
                Piercing = new[] { 0, 1, 2 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item SEWM500 = new Item
            {
                ID = 283,
                Group = ItemGroupEnum.Magnum,
                GroupIndex = 2,
                Name = "S&W M500 (MAG)",
                Alias = "s&wm500",
                Firepower = new[] { 2100, 2300, 2500, 2700, 2900, 3100, 3300, 3500, 3700, 3900, 4100, 4300, 4500, 5000 },
                ReloadSpeed = new[] { 3.53, 3.18 },
                Capacity = new[] { 5, 6 },
                Critical = new[] { 0 },
                Piercing = new[] { 0, 1 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item GL = new Item
            {
                ID = 268,
                Group = ItemGroupEnum.Launcher,
                GroupIndex = 0,
                Name = "Grenade Launcher",
                Alias = "gl",
                Firepower = new[] { 100 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item GL_ICE = new Item
            {
                ID = 295,
                Group = ItemGroupEnum.Launcher,
                GroupIndex = 1,
                Name = "Grenade Launcher (ICE)",
                Alias = "glice",
                Firepower = new[] { 100 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item GL_FLM = new Item
            {
                ID = 313,
                Group = ItemGroupEnum.Launcher,
                GroupIndex = 2,
                Name = "Grenade Launcher (FLM)",
                Alias = "glflm",
                Firepower = new[] { 500 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item GL_ACD = new Item
            {
                ID = 294,
                Group = ItemGroupEnum.Launcher,
                GroupIndex = 3,
                Name = "Grenade Launcher (ACD)",
                Alias = "glacd",
                Firepower = new[] { 500 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item GL_EXP = new Item
            {
                ID = 293,
                Group = ItemGroupEnum.Launcher,
                GroupIndex = 4,
                Name = "Grenade Launcher (EXP)",
                Alias = "glexp",
                Firepower = new[] { 1000 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item GL_FLS = new Item
            {
                ID = 314,
                Group = ItemGroupEnum.Launcher,
                GroupIndex = 5,
                Name = "Grenade Launcher (FLS)",
                Alias = "glfls",
                Firepower = new[] { 100 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item GL_ELC = new Item
            {
                ID = 315,
                Group = ItemGroupEnum.Launcher,
                GroupIndex = 6,
                Name = "Grenade Launcher (ELC)",
                Alias = "glelc",
                Firepower = new[] { 100 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item RPG = new Item
            {
                ID = 269,
                Group = ItemGroupEnum.Launcher,
                GroupIndex = 7,
                Name = "Rocket Launcher",
                Alias = "rocketlauncher",
                Firepower = new[] { 30000 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Stunrod = new Item
            {
                ID = 290,
                Group = ItemGroupEnum.Melee,
                GroupIndex = 0,
                Name = "Stun Rod",
                Alias = "stunrod",
                Firepower = new[] { 666 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item KnifeChris = new Item
            {
                ID = 257,
                Group = ItemGroupEnum.Melee,
                GroupIndex = 1,
                Name = "Knife (Chris)",
                Alias = "knifechris",
                Firepower = new[] { 50 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item KnifeSheva = new Item
            {
                ID = 270,
                Group = ItemGroupEnum.Melee,
                GroupIndex = 2,
                Name = "Knife (Sheva)",
                Alias = "knifesheva",
                Firepower = new[] { 50 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item KnifeJill = new Item
            {
                ID = 292,
                Group = ItemGroupEnum.Melee,
                GroupIndex = 3,
                Name = "Knife (Jill)",
                Alias = "knifejill",
                Firepower = new[] { 50 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item KnifeJillDLC = new Item
            {
                ID = 276,
                Group = ItemGroupEnum.Melee,
                GroupIndex = 4,
                Name = "Knife (Jill) (DLC)",
                Alias = "knifejilldlc",
                Firepower = new[] { 50 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item KnifeWesker = new Item
            {
                ID = 291,
                Group = ItemGroupEnum.Melee,
                GroupIndex = 5,
                Name = "Knife (Wesker)",
                Alias = "knifewesker",
                Firepower = new[] { 50 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item G_EXP = new Item
            {
                ID = 262,
                Group = ItemGroupEnum.Explosive,
                GroupIndex = 0,
                Name = "Hand Grenade",
                Alias = "grenadeexp",
                Firepower = new[] { 1000 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 5 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item G_FLM = new Item
            {
                ID = 263,
                Group = ItemGroupEnum.Explosive,
                GroupIndex = 1,
                Name = "Incendiary Grenade",
                Alias = "grenadeflm",
                Firepower = new[] { 500 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 5 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item G_FLS = new Item
            {
                ID = 264,
                Group = ItemGroupEnum.Explosive,
                GroupIndex = 2,
                Name = "Flash Grenade",
                Alias = "grenadefls",
                Firepower = new[] { 100 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 5 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item ProximityBomb = new Item
            {
                ID = 266,
                Group = ItemGroupEnum.Explosive,
                GroupIndex = 3,
                Name = "Proximity Bomb",
                Alias = "proxmine",
                Firepower = new[] { 1500 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 5 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item HG_Ammo = new Item
            {
                ID = 513,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 0,
                Name = "Handgun Ammo",
                Alias = "ammohandgun",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 50 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item MG_Ammo = new Item
            {
                ID = 514,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 1,
                Name = "Machinegun Ammo",
                Alias = "ammomachinegun",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 150 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item SG_Ammo = new Item
            {
                ID = 515,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 2,
                Name = "Shotgun Ammo",
                Alias = "ammoshotgun",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 30 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item RIF_Ammo = new Item
            {
                ID = 516,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 3,
                Name = "Rifle Ammo",
                Alias = "ammorifle",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 30 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item MAG_Ammo = new Item
            {
                ID = 521,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 4,
                Name = "Magnum Ammo",
                Alias = "ammomagnum",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Rounds_ICE = new Item
            {
                ID = 520,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 5,
                Name = "Nitrogen Rounds",
                Alias = "roundice",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Rounds_FLM = new Item
            {
                ID = 526,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 6,
                Name = "Incendiary Rounds",
                Alias = "roundflm",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Rounds_ACD = new Item
            {
                ID = 519,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 7,
                Name = "Acid Rounds",
                Alias = "roundacd",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Rounds_EXP = new Item
            {
                ID = 518,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 8,
                Name = "Explosive Rounds",
                Alias = "roundexp",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Rounds_FLS = new Item
            {
                ID = 527,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 9,
                Name = "Flash Rounds",
                Alias = "roundfls",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Rounds_ELC = new Item
            {
                ID = 528,
                Group = ItemGroupEnum.Ammunition,
                GroupIndex = 10,
                Name = "Eletric Rounds",
                Alias = "roundelc",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 12 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item FirstAidSpray = new Item
            {
                ID = 772,
                Group = ItemGroupEnum.Heal,
                GroupIndex = 0,
                Name = "First Aid Spray",
                Alias = "firstaidspray",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Herb_G = new Item
            {
                ID = 773,
                Group = ItemGroupEnum.Heal,
                GroupIndex = 1,
                Name = "Herb (Green)",
                Alias = "herbg",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Herb_R = new Item
            {
                ID = 774,
                Group = ItemGroupEnum.Heal,
                GroupIndex = 2,
                Name = "Herb (Red)",
                Alias = "herbr",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Herb_GG = new Item
            {
                ID = 775,
                Group = ItemGroupEnum.Heal,
                GroupIndex = 3,
                Name = "Herb (G+G)",
                Alias = "herbgg",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Herb_GR = new Item
            {
                ID = 777,
                Group = ItemGroupEnum.Heal,
                GroupIndex = 4,
                Name = "Herb (G+R)",
                Alias = "herbgr",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Egg_R = new Item
            {
                ID = 310,
                Group = ItemGroupEnum.EggHeal,
                GroupIndex = 5,
                Name = "Egg (Rotten)",
                Alias = "eggrotten",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 5 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Egg_W = new Item
            {
                ID = 316,
                Group = ItemGroupEnum.EggHeal,
                GroupIndex = 6,
                Name = "Egg (White)",
                Alias = "eggwhite",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 5 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Egg_B = new Item
            {
                ID = 317,
                Group = ItemGroupEnum.EggHeal,
                GroupIndex = 7,
                Name = "Egg (Brown)",
                Alias = "eggbrown",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 5 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Egg_G = new Item
            {
                ID = 318,
                Group = ItemGroupEnum.EggHeal,
                GroupIndex = 8,
                Name = "Egg (Golden)",
                Alias = "egggolden",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 5 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Melee_Vest = new Item
            {
                ID = 1537,
                Group = ItemGroupEnum.Utility,
                GroupIndex = 0,
                Name = "Melee Vest",
                Alias = "meleevest",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item BulletProof_Vest = new Item
            {
                ID = 1542,
                Group = ItemGroupEnum.Utility,
                GroupIndex = 1,
                Name = "Bulletproof Vest",
                Alias = "bulletproofvest",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item RPG7 = new Item
            {
                ID = 309,
                Group = ItemGroupEnum.Special,
                GroupIndex = 0,
                Name = "RPG-7 NVS",
                Alias = "rpg7",
                Firepower = new[] { 30000 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item LTD = new Item
            {
                ID = 308,
                Group = ItemGroupEnum.Special,
                GroupIndex = 1,
                Name = "L.T.D",
                Alias = "ltd",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item GatlingGun = new Item
            {
                ID = 277,
                Group = ItemGroupEnum.Special,
                GroupIndex = 2,
                Name = "Gatling Gun",
                Alias = "gatlinggun",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item LongBow = new Item
            {
                ID = 271,
                Group = ItemGroupEnum.Special,
                GroupIndex = 3,
                Name = "Long Bow",
                Alias = "longbow",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 1 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            Item Flametrower = new Item
            {
                ID = 289,
                Group = ItemGroupEnum.Special,
                GroupIndex = 4,
                Name = "Flametrower",
                Alias = "flametrower",
                Firepower = new[] { 0 },
                ReloadSpeed = new[] { 0.0 },
                Capacity = new[] { 100 },
                Critical = new[] { 0 },
                Piercing = new[] { 0 },
                Range = new[] { 0 },
                Scope = new[] { 0 }
            };

            var obj = new List<Item>
            {
                Default,
                HandToHand,
                M92F,
                HKP8,
                SIGP226,
                M93R,
                Samurai_Edge,
                Samurai_Edge_DLC,
                Px4,
                Ithaca,
                M3,
                JailBreaker,
                Hydra,
                VZ61,
                AK74,
                MP5,
                SIG556,
                S75,
                Dragunov,
                PSG1,
                SEWM29,
                LHAWK,
                SEWM500,
                GL,
                GL_ICE,
                GL_FLM,
                GL_ACD,
                GL_EXP,
                GL_FLS,
                GL_ELC,
                RPG,
                Stunrod,
                KnifeChris,
                KnifeSheva,
                KnifeJill,
                KnifeJillDLC,
                KnifeWesker,
                G_FLM,
                G_EXP,
                G_FLS,
                ProximityBomb,
                HG_Ammo,
                MG_Ammo,
                SG_Ammo,
                RIF_Ammo,
                MAG_Ammo,
                Rounds_ICE,
                Rounds_FLM,
                Rounds_ACD,
                Rounds_EXP,
                Rounds_FLS,
                Rounds_ELC,
                FirstAidSpray,
                Herb_G,
                Herb_R,
                Herb_GG,
                Herb_GR,
                Egg_R,
                Egg_W,
                Egg_B,
                Egg_G,
                Melee_Vest,
                BulletProof_Vest,
                RPG7,
                LTD,
                GatlingGun,
                LongBow,
                Flametrower
            };

            return obj;
        }
    }
}

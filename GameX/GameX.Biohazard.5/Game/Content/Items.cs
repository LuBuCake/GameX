using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameX.Base.Helpers;
using GameX.Base.Modules;
using GameX.Game.Helpers;
using GameX.Game.Types;

namespace GameX.Game.Content
{
    public class Items
    {
        public static List<Item> GetDefaultItems()
        {
            // SRC https://www.evilresource.com/resident-evil-5/weaponry

            Item Default = new Item
            {
                ID = 0,
                Group = (int) ItemGroup.Default,
                GroupIndex = 0,
                Name = "Nothing",
                Portrait = "default",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {0},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item M92F = new Item
            {
                ID = 258,
                Group = (int) ItemGroup.Handgun,
                GroupIndex = 0,
                Name = "M92F (HG)",
                Portrait = "pistol_m92f",
                Firepower = new[] {150, 170, 190, 210, 230, 250},
                ReloadSpeed = new[] {1.70, 1.62, 1.53, 1.36},
                Capacity = new[] {10, 13, 16, 20, 25, 30, 33, 37, 40, 45, 50, 60, 70, 100},
                Critical = new[] {0, 1, 2, 3},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item HKP8 = new Item
            {
                ID = 272,
                Group = (int) ItemGroup.Handgun,
                GroupIndex = 1,
                Name = "H&K P8 (HG)",
                Portrait = "pistol_h&k_p8",
                Firepower = new[] {140, 160, 180, 200, 220, 240, 260, 300},
                ReloadSpeed = new[] {1.53, 1.36, 1.19, 1.11, 1.02, 0.95},
                Capacity = new[] {9, 11, 13, 15, 17, 19, 21, 25},
                Critical = new[] {0},
                Piercing = new[] {0, 1, 2, 3},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item SIGP226 = new Item
            {
                ID = 273,
                Group = (int) ItemGroup.Handgun,
                GroupIndex = 2,
                Name = "SIG P226 (HG)",
                Portrait = "pistol_sig_p226",
                Firepower = new[] {180, 220, 240, 260, 280, 300, 320, 340, 350, 370, 390, 410, 430, 480},
                ReloadSpeed = new[] {1.70, 1.62},
                Capacity = new[] {8, 10, 12, 13, 14, 16},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item M93R = new Item
            {
                ID = 286,
                Group = (int) ItemGroup.Handgun,
                GroupIndex = 3,
                Name = "M93R (HG)",
                Portrait = "pistol_m93r",
                Firepower = new[] {170, 190, 210, 230, 250, 270, 290, 310, 330, 350, 370, 400},
                ReloadSpeed = new[] {1.70, 1.62, 1.53, 1.36},
                Capacity = new[] {10, 12, 14, 16, 18, 20, 22, 24, 26, 30},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Samurai_Edge = new Item
            {
                ID = 297,
                Group = (int) ItemGroup.Handgun,
                GroupIndex = 4,
                Name = "Samurai Edge (HG)",
                Portrait = "pistol_samurai_edge",
                Firepower = new[] {400},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {15},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Samurai_Edge_DLC = new Item
            {
                ID = 274,
                Group = (int) ItemGroup.Handgun,
                GroupIndex = 5,
                Name = "Samurai Edge DLC (HG)",
                Portrait = "pistol_samurai_edge_dlc",
                Firepower = new[] {340},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {30},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Px4 = new Item
            {
                ID = 287,
                Group = (int) ItemGroup.Handgun,
                GroupIndex = 6,
                Name = "Px4 (HG)",
                Portrait = "pistol_px4",
                Firepower = new[] {300},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {25},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Ithaca = new Item
            {
                ID = 260,
                Group = (int) ItemGroup.Shotgun,
                GroupIndex = 0,
                Name = "Ithaca (SG)",
                Portrait = "shotgun_ithaca",
                Firepower = new[] {200, 230, 260, 300, 330, 360, 400},
                ReloadSpeed = new[] {3.00, 2.85, 2.70, 2.40},
                Capacity = new[] {6, 7, 8, 9, 10, 12, 13, 15, 16, 17, 18, 20, 22, 25},
                Critical = new[] {0, 1},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item M3 = new Item
            {
                ID = 278,
                Group = (int) ItemGroup.Shotgun,
                GroupIndex = 1,
                Name = "M3 (SG)",
                Portrait = "shotgun_m3",
                Firepower = new[] {300, 320, 350, 370, 400, 420, 450, 480, 500, 550, 600, 650, 700, 900},
                ReloadSpeed = new[] {3.00, 2.70},
                Capacity = new[] {5, 6, 7, 8, 9, 10},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item JailBreaker = new Item
            {
                ID = 279,
                Group = (int) ItemGroup.Shotgun,
                GroupIndex = 2,
                Name = "Jail Breaker (SG)",
                Portrait = "shotgun_jailbreaker",
                Firepower = new[] {180, 200, 220, 250, 300, 350},
                ReloadSpeed = new[] {2.52, 2.38, 2.24, 2.10, 1.96, 1.82},
                Capacity = new[] {5, 6, 7, 8, 9, 10, 11, 12, 13, 15},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0, 1, 2},
                Scope = new[] {0}
            };

            Item Hydra = new Item
            {
                ID = 281,
                Group = (int) ItemGroup.Shotgun,
                GroupIndex = 3,
                Name = "Hydra (SG)",
                Portrait = "shotgun_hydra",
                Firepower = new[] {280, 290, 310, 330, 350, 380, 400, 420, 440, 460, 500, 550},
                ReloadSpeed = new[] {3.67, 3.30},
                Capacity = new[] {4, 5, 6, 7, 8, 9, 10},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0, 1, 2},
                Scope = new[] {0}
            };

            Item VZ61 = new Item
            {
                ID = 259,
                Group = (int) ItemGroup.MachineGun,
                GroupIndex = 0,
                Name = "VZ61 (MG)",
                Portrait = "smg_vz61",
                Firepower = new[] {50, 60, 80, 100},
                ReloadSpeed = new[] {2.83, 2.69, 2.55, 2.27},
                Capacity = new[] {50, 60, 80, 100, 120, 140, 160, 180, 200, 220, 240, 260, 280, 300},
                Critical = new[] {0, 1, 2},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item AK74 = new Item
            {
                ID = 285,
                Group = (int) ItemGroup.MachineGun,
                GroupIndex = 1,
                Name = "AK74  (MG)",
                Portrait = "smg_ak74",
                Firepower = new[] {90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 190, 200, 220, 250},
                ReloadSpeed = new[] {2.83, 2.55},
                Capacity = new[] {30, 35, 40, 45, 50},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item MP5 = new Item
            {
                ID = 275,
                Group = (int) ItemGroup.MachineGun,
                GroupIndex = 2,
                Name = "H&K MP5 (MG)",
                Portrait = "smg_mp5",
                Firepower = new[] {60, 80, 90, 100, 120},
                ReloadSpeed = new[] {2.70, 2.55, 2.40, 2.10},
                Capacity = new[] {45, 55, 65, 80, 90, 100, 110, 120, 130, 150},
                Critical = new[] {0},
                Piercing = new[] {0, 1, 2},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item SIG556 = new Item
            {
                ID = 265,
                Group = (int) ItemGroup.MachineGun,
                GroupIndex = 3,
                Name = "SIG 556 (MG)",
                Portrait = "smg_sig_556",
                Firepower = new[] {80, 90, 100, 120, 130, 140, 150, 160, 180},
                ReloadSpeed = new[] {2.55, 2.41, 2.27, 1.98, 1.70, 1.42},
                Capacity = new[] {40, 45, 50, 55, 60, 65, 70, 80},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item S75 = new Item
            {
                ID = 261,
                Group = (int) ItemGroup.Rifle,
                GroupIndex = 0,
                Name = "S75 (RIF)",
                Portrait = "rifle_s75",
                Firepower = new[] {750, 800, 850, 900, 950, 1050, 1120, 1200, 1270, 1350, 1420, 1500, 1700, 2000},
                ReloadSpeed = new[] {3.67, 3.30},
                Capacity = new[] {6, 7, 8, 10, 12, 15, 17, 20, 22, 25, 40, 50},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Dragunov = new Item
            {
                ID = 288,
                Group = (int) ItemGroup.Rifle,
                GroupIndex = 1,
                Name = "Dragunov SVD (RIF)",
                Portrait = "rifle_dragunov",
                Firepower = new[] {650, 700, 750, 800, 850, 900, 950, 1000, 1100, 1300},
                ReloadSpeed = new[] {2.83, 2.69, 2.55, 2.41},
                Capacity = new[] {7, 9, 10, 12, 13, 15, 16, 18},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0,},
                Scope = new[] {0}
            };

            Item PSG1 = new Item
            {
                ID = 284,
                Group = (int) ItemGroup.Rifle,
                GroupIndex = 2,
                Name = "H&K PSG-1 (RIF)",
                Portrait = "rifle_h&k_psg1",
                Firepower = new[] {600, 650, 700, 750, 800, 900, 1000, 1200},
                ReloadSpeed = new[] {2.55, 2.41, 2.27, 2.13, 1.98, 1.70},
                Capacity = new[] {5, 6, 7, 9, 11, 15},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0, 1, 2}
            };

            Item SEWM29 = new Item
            {
                ID = 267,
                Group = (int) ItemGroup.Magnum,
                GroupIndex = 0,
                Name = "S&W M29 (MAG)",
                Portrait = "magnum_s&w_m29",
                Firepower = new[] {1500, 1700, 1900, 2100, 2400, 2700, 3200},
                ReloadSpeed = new[] {3.53, 3.36, 3.18, 2.83},
                Capacity = new[] {6, 7, 8, 9, 10, 11, 12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0, 1},
                Scope = new[] {0}
            };

            Item LHAWK = new Item
            {
                ID = 282,
                Group = (int) ItemGroup.Magnum,
                GroupIndex = 1,
                Name = "L. Hawk (MAG)",
                Portrait = "magnum_L_hawk",
                Firepower = new[] {1400, 1600, 1800, 2000, 2300, 2600, 3000},
                ReloadSpeed = new[] {1.70, 1.62, 1.53, 1.45, 1.36},
                Capacity = new[] {5, 6, 7, 8},
                Critical = new[] {0},
                Piercing = new[] {0, 1, 2},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item SEWM500 = new Item
            {
                ID = 283,
                Group = (int) ItemGroup.Magnum,
                GroupIndex = 2,
                Name = "S&W M500 (MAG)",
                Portrait = "magnum_s&w_m500",
                Firepower = new[] {2100, 2300, 2500, 2700, 2900, 3100, 3300, 3500, 3700, 3900, 4100, 4300, 4500, 5000},
                ReloadSpeed = new[] {3.53, 3.18},
                Capacity = new[] {5, 6},
                Critical = new[] {0},
                Piercing = new[] {0, 1},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item GL_ICE = new Item
            {
                ID = 295,
                Group = (int) ItemGroup.Launcher,
                GroupIndex = 0,
                Name = "Grenade Launcher (ICE)",
                Portrait = "launcher_ice",
                Firepower = new[] {100},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item GL_FLM = new Item
            {
                ID = 313,
                Group = (int) ItemGroup.Launcher,
                GroupIndex = 1,
                Name = "Grenade Launcher (FLM)",
                Portrait = "launcher_flm",
                Firepower = new[] {500},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item GL_ACD = new Item
            {
                ID = 294,
                Group = (int) ItemGroup.Launcher,
                GroupIndex = 2,
                Name = "Grenade Launcher (ACD)",
                Portrait = "launcher_acd",
                Firepower = new[] {500},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item GL_EXP = new Item
            {
                ID = 293,
                Group = (int) ItemGroup.Launcher,
                GroupIndex = 3,
                Name = "Grenade Launcher (EXP)",
                Portrait = "launcher_exp",
                Firepower = new[] {1000},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item GL_FLS = new Item
            {
                ID = 314,
                Group = (int) ItemGroup.Launcher,
                GroupIndex = 4,
                Name = "Grenade Launcher (FLS)",
                Portrait = "launcher_fls",
                Firepower = new[] {100},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item GL_ELC = new Item
            {
                ID = 315,
                Group = (int) ItemGroup.Launcher,
                GroupIndex = 5,
                Name = "Grenade Launcher (ELC)",
                Portrait = "launcher_elc",
                Firepower = new[] {100},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item RPG = new Item
            {
                ID = 269,
                Group = (int) ItemGroup.Launcher,
                GroupIndex = 6,
                Name = "Rocket Launcher",
                Portrait = "rocket_launcher",
                Firepower = new[] {30000},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Stunrod = new Item
            {
                ID = 290,
                Group = (int) ItemGroup.Melee,
                GroupIndex = 0,
                Name = "Stun Rod",
                Portrait = "stunrod",
                Firepower = new[] {666},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item KnifeChris = new Item
            {
                ID = 257,
                Group = (int) ItemGroup.Melee,
                GroupIndex = 1,
                Name = "Knife (Chris)",
                Portrait = "knife_chris",
                Firepower = new[] {50},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item KnifeSheva = new Item
            {
                ID = 270,
                Group = (int) ItemGroup.Melee,
                GroupIndex = 2,
                Name = "Knife (Sheva)",
                Portrait = "knife_sheva",
                Firepower = new[] {50},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item KnifeJill = new Item
            {
                ID = 292,
                Group = (int) ItemGroup.Melee,
                GroupIndex = 3,
                Name = "Knife (Jill)",
                Portrait = "knife_jill",
                Firepower = new[] {50},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item KnifeJillDLC = new Item
            {
                ID = 276,
                Group = (int) ItemGroup.Melee,
                GroupIndex = 4,
                Name = "Knife (Jill) (DLC)",
                Portrait = "knife_jill_dlc",
                Firepower = new[] {50},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item KnifeWesker = new Item
            {
                ID = 291,
                Group = (int) ItemGroup.Melee,
                GroupIndex = 5,
                Name = "Knife (Wesker)",
                Portrait = "knife_wesker",
                Firepower = new[] {50},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item G_EXP = new Item
            {
                ID = 262,
                Group = (int) ItemGroup.Explosive,
                GroupIndex = 0,
                Name = "Hand Grenade",
                Portrait = "grenade_exp",
                Firepower = new[] {1000},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {5},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item G_FLM = new Item
            {
                ID = 263,
                Group = (int) ItemGroup.Explosive,
                GroupIndex = 1,
                Name = "Incendiary Grenade",
                Portrait = "grenade_flm",
                Firepower = new[] {500},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {5},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item G_FLS = new Item
            {
                ID = 264,
                Group = (int) ItemGroup.Explosive,
                GroupIndex = 2,
                Name = "Flash Grenade",
                Portrait = "grenade_fls",
                Firepower = new[] {100},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {5},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item ProximityBomb = new Item
            {
                ID = 266,
                Group = (int) ItemGroup.Explosive,
                GroupIndex = 3,
                Name = "Proximity Bomb",
                Portrait = "proximity_bomb",
                Firepower = new[] {1500},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {5},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item HG_Ammo = new Item
            {
                ID = 513,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 0,
                Name = "Handgun Ammo",
                Portrait = "ammo_handgun",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {50},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item MG_Ammo = new Item
            {
                ID = 514,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 1,
                Name = "Machinegun Ammo",
                Portrait = "ammo_machinegun",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {150},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item SG_Ammo = new Item
            {
                ID = 515,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 2,
                Name = "Shotgun Ammo",
                Portrait = "ammo_shotgun",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {30},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item RIF_Ammo = new Item
            {
                ID = 516,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 3,
                Name = "Rifle Ammo",
                Portrait = "ammo_rifle",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {30},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item MAG_Ammo = new Item
            {
                ID = 521,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 4,
                Name = "Magnum Ammo",
                Portrait = "ammo_magnum",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Rounds_ICE = new Item
            {
                ID = 520,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 5,
                Name = "Nitrogen Rounds",
                Portrait = "rounds_ice",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Rounds_FLM = new Item
            {
                ID = 526,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 6,
                Name = "Incendiary Rounds",
                Portrait = "rounds_flm",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Rounds_ACD = new Item
            {
                ID = 519,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 7,
                Name = "Acid Rounds",
                Portrait = "rounds_acd",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Rounds_EXP = new Item
            {
                ID = 518,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 8,
                Name = "Explosive Rounds",
                Portrait = "rounds_exp",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Rounds_FLS = new Item
            {
                ID = 527,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 9,
                Name = "Flash Rounds",
                Portrait = "rounds_fls",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Rounds_ELC = new Item
            {
                ID = 528,
                Group = (int) ItemGroup.Ammunition,
                GroupIndex = 10,
                Name = "Eletric Rounds",
                Portrait = "rounds_elc",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {12},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item FirstAidSpray = new Item
            {
                ID = 772,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 0,
                Name = "First Aid Spray",
                Portrait = "item_firstaidspray",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Herb_G = new Item
            {
                ID = 773,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 1,
                Name = "Herb (Green)",
                Portrait = "herb_g",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Herb_R = new Item
            {
                ID = 774,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 2,
                Name = "Herb (Red)",
                Portrait = "herb_r",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Herb_GG = new Item
            {
                ID = 775,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 3,
                Name = "Herb (G+G)",
                Portrait = "herb_gg",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Herb_GR = new Item
            {
                ID = 777,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 4,
                Name = "Herb (G+R)",
                Portrait = "herb_gr",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Egg_R = new Item
            {
                ID = 310,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 5,
                Name = "Egg (Rotten)",
                Portrait = "egg_rotten",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {5},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Egg_W = new Item
            {
                ID = 316,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 6,
                Name = "Egg (White)",
                Portrait = "egg_white",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {5},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Egg_B = new Item
            {
                ID = 317,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 7,
                Name = "Egg (Brown)",
                Portrait = "egg_brown",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {5},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Egg_G = new Item
            {
                ID = 318,
                Group = (int) ItemGroup.Heal,
                GroupIndex = 8,
                Name = "Egg (Golden)",
                Portrait = "egg_golden",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {5},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Melee_Vest = new Item
            {
                ID = 1537,
                Group = (int) ItemGroup.Utility,
                GroupIndex = 0,
                Name = "Melee Vest",
                Portrait = "vest_melee",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item BulletProof_Vest = new Item
            {
                ID = 1542,
                Group = (int) ItemGroup.Utility,
                GroupIndex = 1,
                Name = "Bulletproof Vest",
                Portrait = "vest_bulletproof",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item RPG7 = new Item
            {
                ID = 309,
                Group = (int) ItemGroup.Special,
                GroupIndex = 0,
                Name = "RPG-7 NVS",
                Portrait = "rocket_launcher_special",
                Firepower = new[] {30000},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item LTD = new Item
            {
                ID = 308,
                Group = (int) ItemGroup.Special,
                GroupIndex = 1,
                Name = "L.T.D",
                Portrait = "ltd",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item GatlingGun = new Item
            {
                ID = 277,
                Group = (int) ItemGroup.Special,
                GroupIndex = 2,
                Name = "Gatling Gun",
                Portrait = "gatling_gun",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item LongBow = new Item
            {
                ID = 271,
                Group = (int) ItemGroup.Special,
                GroupIndex = 3,
                Name = "Long Bow",
                Portrait = "longbow",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Lantern = new Item
            {
                ID = 304,
                Group = (int) ItemGroup.Special,
                GroupIndex = 4,
                Name = "Lantern",
                Portrait = "lantern",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {1},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            Item Flametrower = new Item
            {
                ID = 289,
                Group = (int) ItemGroup.Special,
                GroupIndex = 4,
                Name = "Flametrower",
                Portrait = "flametrower",
                Firepower = new[] {0},
                ReloadSpeed = new[] {0.0},
                Capacity = new[] {100},
                Critical = new[] {0},
                Piercing = new[] {0},
                Range = new[] {0},
                Scope = new[] {0}
            };

            return new List<Item>
            {
                Default,
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
                Lantern,
                Flametrower
            };
        }

        public static void WriteDefaultItems()
        {
            List<Item> Chars = GetDefaultItems();

            try
            {
                foreach (Item item in Chars)
                {
                    Serializer.WriteDataFile(@"addons/GameX.Biohazard.5/prefabs/item/" + $"{item.Portrait.ToLower()}.json", Serializer.SerializeItem(item));
                }

                Terminal.WriteLine("[App] Item jsons written sucessfully.");
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
            }
        }

        public static List<Item> GetItemsFromFolder()
        {
            try
            {
                DirectoryInfo ItemsFolder = new DirectoryInfo(@"addons/GameX.Biohazard.5/prefabs/item/");
                FileInfo[] ItemFiles = ItemsFolder.GetFiles("*.json");
                List<Item> AvailableItems = new List<Item>();

                foreach (FileInfo ItemFile in ItemFiles)
                {
                    AvailableItems.Add(Serializer.DeserializeItem(File.ReadAllText(@"addons/GameX.Biohazard.5/prefabs/item/" + ItemFile.Name)));
                }

                return AvailableItems.OrderBy(x => x.Group).ThenBy(x => x.GroupIndex).ToList();
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex.Message);
                return new List<Item>();
            }
        }
    }
}
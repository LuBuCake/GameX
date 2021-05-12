using GameX.Base.Modules;
using GameX.Base.Types;

namespace GameX.Game.Modules
{
    public class Biohazard
    {
        public static bool ModuleStarted { get; set; }

        public static void StartModule()
        {
            ModuleStarted = true;
            Terminal.WriteLine("[Biohazard] Module started successfully.");
        }

        public static void FinishModule()
        {
            ModuleStarted = false;
            Terminal.WriteLine("[Biohazard] Module finished successfully.");
        }

        /* Detour Mods */

        public static void InfiniteHealth(bool Enable)
        {
            if (Enable && !Memory.DetourActive("MOD.Infinite_Health"))
            {
                byte[] DetourClean =
                {
                    0xC7, 0x41, 0x14, 0x00, 0x40, 0x1C, 0x46, 0xF3, 0x0F, 0x10, 0x41, 0x14
                };

                byte[] CallInstruction =
                {
                    0xF3, 0x0F, 0x10, 0x41, 0x14
                };

                long CallAddress = Memory.ReadPointer("re8demo.exe", 0x2ED42CB);
                Memory.CreateDetour("MOD.Infinite_Health", DetourClean, CallAddress, CallInstruction, true, 0x142ED42D0);

                return;
            }

            Memory.RemoveDetour("MOD.Infinite_Health");
        }

        public static int GetPCPoints()
        {
            return Memory.ReadInt32("re8.exe", 0x0A1B1B08, 0x60, 0x18);
        }

        public static void SetPCPoints(int Value)
        {
            Memory.WriteInt32(Value, "re8.exe", 0x0A1B1B08, 0x60, 0x18);
        }
    }
}
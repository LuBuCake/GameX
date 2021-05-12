using System;
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

        public static void CustomFOV_Inject(bool Enable)
        {
            if (Enable && !Memory.DetourActive("MOD.Custom_FOV"))
            {
                byte[] DetourClean =
                {
                    0xF3, 0x0F, 0x10, 0x05, 0x05, 0x00, 0x00, 0x00,
                    0xE9, 0X00, 0X00, 0X00, 0X00,
                    0x00, 0x00, 0x00, 0x00
                };

                byte[] CallInstruction =
                {
                    0xF3, 0x0F, 0x10, 0x40, 0x38
                };

                long CallAddress = Memory.ReadPointer("re8.exe", 0x147B44D);
                Detour Custom_FOV = Memory.CreateDetour("MOD.Custom_FOV", DetourClean, CallAddress, CallInstruction);

                if (Custom_FOV == null) 
                    return;

                byte[] jmp = Memory.DetourJump(Custom_FOV.Address() + 8, Custom_FOV.CallAddress() + 5, 5, 5, true);
                Memory.WriteRawAddress(Custom_FOV.Address() + 9, jmp);
                CustomFOV_Update(81);

                return;
            }

            Memory.RemoveDetour("MOD.Custom_FOV");
        }

        public static void CustomFOV_Update(float Value)
        {
            if (!Memory.DetourActive("MOD.Custom_FOV"))
                return;

            byte[] ByteArray = BitConverter.GetBytes(Value);
            Detour Custom_FOV = Memory.GetDetour("MOD.Custom_FOV");
            Memory.WriteRawAddress(Custom_FOV.Address() + 13, ByteArray);
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
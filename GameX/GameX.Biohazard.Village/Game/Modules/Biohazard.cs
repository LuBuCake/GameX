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

        public static void CustomFOVNormal_Inject(bool Enable)
        {
            if (Enable && !Memory.DetourActive("MOD.Custom_FOVNormal"))
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
                Detour Custom_FOV = Memory.CreateDetour("MOD.Custom_FOVNormal", DetourClean, CallAddress, CallInstruction);

                if (Custom_FOV == null) 
                    return;

                byte[] jmp = Memory.DetourJump(Custom_FOV.Address() + 8, Custom_FOV.CallAddress() + 5, 5, 5, true);
                Memory.WriteRawAddress(Custom_FOV.Address() + 9, jmp);
                CustomFOVNormal_SetValue(81);

                return;
            }

            Memory.RemoveDetour("MOD.Custom_FOVNormal");
        }

        public static void CustomFOVNormal_SetValue(float Value)
        {
            if (!Memory.DetourActive("MOD.Custom_FOVNormal"))
                return;

            byte[] ByteArray = BitConverter.GetBytes(Value);
            Detour Custom_FOV = Memory.GetDetour("MOD.Custom_FOVNormal");
            Memory.WriteRawAddress(Custom_FOV.Address() + 13, ByteArray);
        }

        public static void CustomFOVAiming_Inject(bool Enable)
        {
            if (Enable && !Memory.DetourActive("MOD.Custom_FOVAiming"))
            {
                byte[] DetourClean =
                {
                    0xF3, 0x0F, 0x10, 0x05, 0x05, 0x00, 0x00, 0x00,
                    0xE9, 0X00, 0X00, 0X00, 0X00,
                    0x00, 0x00, 0x00, 0x00
                };

                byte[] CallInstruction =
                {
                    0xF3, 0x0F, 0x10, 0x40, 0x3C
                };

                long CallAddress = Memory.ReadPointer("re8.exe", 0x147B48D);
                Detour Custom_FOV = Memory.CreateDetour("MOD.Custom_FOVAiming", DetourClean, CallAddress, CallInstruction);

                if (Custom_FOV == null)
                    return;

                byte[] jmp = Memory.DetourJump(Custom_FOV.Address() + 8, Custom_FOV.CallAddress() + 5, 5, 5, true);
                Memory.WriteRawAddress(Custom_FOV.Address() + 9, jmp);
                CustomFOVAiming_SetValue(70);

                return;
            }

            Memory.RemoveDetour("MOD.Custom_FOVAiming");
        }

        public static void CustomFOVAiming_SetValue(float Value)
        {
            if (!Memory.DetourActive("MOD.Custom_FOVAiming"))
                return;

            byte[] ByteArray = BitConverter.GetBytes(Value);
            Detour Custom_FOV = Memory.GetDetour("MOD.Custom_FOVAiming");
            Memory.WriteRawAddress(Custom_FOV.Address() + 13, ByteArray);
        }

        public static int GetCPPoints()
        {
            return Memory.ReadInt32("re8.exe", 0x0A1B1B08, 0x60, 0x18);
        }

        public static void SetCPPoints(int Value)
        {
            Memory.WriteInt32(Value, "re8.exe", 0x0A1B1B08, 0x60, 0x18);
        }

        public static int GetLei()
        {
            return Memory.ReadInt32("re8.exe", 0x0A1BB500, 0x40, 0x78, 0x48);
        }

        public static void SetLei(int Value)
        {
            Memory.WriteInt32(Value, "re8.exe", 0x0A1BB500, 0x40, 0x78, 0x48);
        }

        public static void CraftCheck(bool Disable)
        {
            Memory.WriteBytes(Disable ? new byte[]{ 0xEB } : new byte[] { 0x7B }, "re8.exe", 0x109038B);
        }

        public static void Flashlight(bool Enable)
        {
            Memory.WriteBytes(Enable ? new byte[] { 0xB0, 0X01, 0X90 } : new byte[] { 0x0F, 0X9F, 0XC0 }, "re8.exe", 0x128EE5E);
        }
    }
}
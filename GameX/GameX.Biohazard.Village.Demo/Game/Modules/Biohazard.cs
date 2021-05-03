using GameX.Base.Modules;

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

        public static void NoTimeDecrease(bool Enable)
        {
            Memory.WriteBytes(Enable ? new byte[] { 0x90, 0x90, 0x90, 0x90 } : new byte[] { 0xC6, 0x43, 0x20, 0x01 }, "re8demo.exe", 0x47EB9F);
        }
    }
}
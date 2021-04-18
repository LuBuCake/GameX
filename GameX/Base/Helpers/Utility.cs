using GameX.Base.Modules;
using System;
using System.Drawing;
using System.Linq;

namespace GameX.Base.Helpers
{
    public static class Utility
    {
        public static int Clamp(int Value, int Min, int Max)
        {
            if (Value > Max) return Max;
            else if (Value < Min) return Min;
            return Value;
        }

        public static bool CompareByteArray(byte[] Array1, byte[] Array2, int Length)
        {
            if (Array1.Length != Array2.Length)
                return false;

            for (int i = 0; i < Length; i++)
                if (Array1[i] != Array1[i])
                    return false;

            return true;
        }

        public static string RemoveWhiteSpace(string Source)
        {
            return new string(Source.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }

        public static Image GetImageFromFile(string File)
        {
            try
            {
                Image img = Image.FromFile(File);
                return img;
            }
            catch (Exception)
            {
                Terminal.WriteLine($"Portrait file not found: {File}");
                return null;
            }
        }
    }
}

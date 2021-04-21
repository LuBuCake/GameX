using GameX.Base.Modules;
using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GameX.Base.Helpers
{
    public static class Utility
    {
        public static int Clamp(int Value, int Min, int Max)
        {
            return Value > Max ? Max : Value < Min ? Min : Value;
        }

        public static bool CompareByteArray(byte[] Array1, byte[] Array2, int Length)
        {
            if (Array1.Length != Array2.Length)
                return false;

            for (int i = 0; i < Length; i++)
                if (Array1[i] != Array2[i])
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
                Terminal.WriteLine($"Image file not found: {File}");
                return null;
            }
        }

        public static Image GetImageFromStream(string File)
        {
            try
            {
                Image img = Image.FromStream(new MemoryStream(Encoder.GetDecodedStream(File)));
                return img;
            }
            catch (Exception)
            {
                Terminal.WriteLine($"Image file not found: {File}");
                return null;
            }
        }

        public static void MessageBox_Information(string Message)
        {
            MessageBox.Show(Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void MessageBox_Error(string Message)
        {
            MessageBox.Show(Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void MessageBox_Warning(string Message)
        {
            MessageBox.Show(Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GameX.Modules;

namespace GameX.Helpers
{
    public static class Utility
    {
        public static int Clamp(int Value, int Min, int Max)
        {
            return Value > Max ? Max : Value < Min ? Min : Value;
        }

        public static double Clamp(double Value, double Min, double Max)
        {
            return Value > Max ? Max : Value < Min ? Min : Value;
        }

        public static float Lerp(float Delta, float From, float To)
        {
            return From * (1 - Delta) + To * Delta;
        }

        public static double Lerp(double Delta, double From, double To)
        {
            return From * (1 - Delta) + To * Delta;
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

        /* OBS: DevExpress MessageBoxes can cause problems if used logically in a lopped routine */

        public static DialogResult MessageBox_Information(string Message, MessageBoxButtons Button = MessageBoxButtons.OK)
        {
            return XtraMessageBox.Show(Message, "Information", Button, MessageBoxIcon.Information);
        }

        public static DialogResult MessageBox_Error(string Message, MessageBoxButtons Button = MessageBoxButtons.OK)
        {
            return XtraMessageBox.Show(Message, "Error", Button, MessageBoxIcon.Error);
        }

        public static DialogResult MessageBox_Warning(string Message, MessageBoxButtons Button = MessageBoxButtons.OK)
        {
            return XtraMessageBox.Show(Message, "Warning", Button, MessageBoxIcon.Warning);
        }
    }
}
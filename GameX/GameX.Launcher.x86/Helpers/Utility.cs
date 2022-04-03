using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GameX.Launcher.Helpers
{
    public static class Utility
    {
        public static int Clamp(int Value, int Min, int Max)
        {
            return Value > Max ? Max : Value < Min ? Min : Value;
        }

        public static bool TestConnection(string HostNameOrAddress, int Timeout = 1000)
        {
            try
            {
                Ping myPing = new Ping();
                PingReply reply = myPing.Send(HostNameOrAddress, Timeout, new byte[32]);
                return (reply.Status == IPStatus.Success);
            }
            catch (Exception)
            {
                return false;
            }
        }

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
using System;
using System.Drawing;
using System.IO;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GameX.Launcher.Base.Helpers
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

        public static Image GetImageFromStream(string File)
        {
            try
            {
                Image img = Image.FromStream(new MemoryStream(Encoder.GetDecodedStream(File)));
                return img;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static Image ColorReplace(this Image inputImage, Color NewColor, bool IgnoreAlpha = false)
        {
            Bitmap outputImage = new Bitmap(inputImage.Width, inputImage.Height);
            Bitmap inputImageBitmap = new Bitmap(inputImage);

            for (int y = 0; y < outputImage.Height; y++)
            {
                for (int x = 0; x < outputImage.Width; x++)
                {
                    if (IgnoreAlpha)
                    {
                        Color CurrentColor = inputImageBitmap.GetPixel(x, y);
                        outputImage.SetPixel(x, y, Color.FromArgb(CurrentColor.A, NewColor.R, NewColor.G, NewColor.B));
                    }
                    else
                    {
                        outputImage.SetPixel(x, y, NewColor);
                    }
                }
            }

            return outputImage;
        }

        public static Image MergeImage(Image BaseImage, params Image[] SubsequentImages)
        {
            Bitmap outputImage = new Bitmap(BaseImage);

            for (int i = 0; i < SubsequentImages.Length; i++)
            {
                Bitmap currentBitmap = new Bitmap(SubsequentImages[i]);

                for (int y = 0; y < outputImage.Height; y++)
                {
                    for (int x = 0; x < outputImage.Width; x++)
                    {
                        Color CurrentColor = outputImage.GetPixel(x, y);
                        Color ColorToAdd = currentBitmap.GetPixel(x, y);
                        Color NewColor = Color.FromArgb(Clamp(CurrentColor.A + ColorToAdd.A, 0, 255), Clamp(CurrentColor.R + ColorToAdd.R, 0, 255), Clamp(CurrentColor.G + ColorToAdd.G, 0, 255), Clamp(CurrentColor.B + ColorToAdd.B, 0, 255));

                        outputImage.SetPixel(x, y, NewColor);
                    }
                }
            }

            return outputImage;
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
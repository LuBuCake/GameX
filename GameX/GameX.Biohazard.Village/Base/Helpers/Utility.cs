using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using GameX.Base.Modules;

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
            return MessageBox.Show(Message, "Information", Button, MessageBoxIcon.Information);
        }

        public static DialogResult MessageBox_Error(string Message, MessageBoxButtons Button = MessageBoxButtons.OK)
        {
            return MessageBox.Show(Message, "Error", Button, MessageBoxIcon.Error);
        }

        public static DialogResult MessageBox_Warning(string Message, MessageBoxButtons Button = MessageBoxButtons.OK)
        {
            return MessageBox.Show(Message, "Warning", Button, MessageBoxIcon.Warning);
        }

        public static DialogResult MessageBox_YesNo(string Message, MessageBoxButtons Button = MessageBoxButtons.OK)
        {
            return MessageBox.Show(Message, "Warning", Button, MessageBoxIcon.Information);
        }
    }
}
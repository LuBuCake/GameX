using System;
using System.Collections.Generic;
using System.Drawing;

namespace GameX.Launcher.Helpers
{
    public static class ImageHelper
    {
        private static Dictionary<string, Image> ImageCache;

        public static Image GetImageFromFile(string File)
        {
            try
            {
                if (ImageCache == null)
                    ImageCache = new Dictionary<string, Image>();

                if (ImageCache.TryGetValue(File, out Image CachedImage))
                    return CachedImage;

                Image img = Image.FromFile(File);
                ImageCache.Add(File, img);

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
                        Color NewColor = Color.FromArgb(Utility.Clamp(CurrentColor.A + ColorToAdd.A, 0, 255), Utility.Clamp(CurrentColor.R + ColorToAdd.R, 0, 255), Utility.Clamp(CurrentColor.G + ColorToAdd.G, 0, 255), Utility.Clamp(CurrentColor.B + ColorToAdd.B, 0, 255));

                        outputImage.SetPixel(x, y, NewColor);
                    }
                }
            }

            return outputImage;
        }
    }
}

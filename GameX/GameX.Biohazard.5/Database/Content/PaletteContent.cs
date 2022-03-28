using System.Collections.Generic;
using DevExpress.LookAndFeel;
using GameX.Database.Type;

namespace GameX.Database.Content
{
    public static class PaletteContent
    {
        public static List<Simple> GetCollection(string SkinName)
        {
            Dictionary<string, SkinSvgPalette> PaletteSet = new Dictionary<string, SkinSvgPalette>();

            if (SkinName == "The Bezier")
            {
                foreach (KeyValuePair<string, SkinSvgPalette> Pallet in SkinSvgPalette.Bezier.PaletteSet)
                    PaletteSet.Add(Pallet.Key, Pallet.Value);
            }
            else if (SkinName == "Basic")
            {
                foreach (KeyValuePair<string, SkinSvgPalette> Pallet in SkinSvgPalette.DefaultSkin.PaletteSet)
                    PaletteSet.Add(Pallet.Key, Pallet.Value);
            }
            else if (SkinName == "Office 2019 Colorful")
            {
                foreach (KeyValuePair<string, SkinSvgPalette> Pallet in SkinSvgPalette.Office2019Colorful.PaletteSet)
                    PaletteSet.Add(Pallet.Key, Pallet.Value);
            }
            else if (SkinName == "Office 2019 Black")
            {
                foreach (KeyValuePair<string, SkinSvgPalette> Pallet in SkinSvgPalette.Office2019Black.PaletteSet)
                    PaletteSet.Add(Pallet.Key, Pallet.Value);
            }
            else if (SkinName == "Office 2019 White")
            {
                foreach (KeyValuePair<string, SkinSvgPalette> Pallet in SkinSvgPalette.Office2019White.PaletteSet)
                    PaletteSet.Add(Pallet.Key, Pallet.Value);
            }

            List<Simple> obj = new List<Simple>();
            int StartIndex = 0;

            foreach (KeyValuePair<string, SkinSvgPalette> Pallet in PaletteSet)
            {
                obj.Add(new Simple(Pallet.Key));
                StartIndex++;
            }

            return obj;
        }
    }
}

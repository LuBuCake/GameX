using DevExpress.LookAndFeel;
using DevExpress.Skins;
using GameX.Types;
using System.Collections.Generic;

namespace GameX.Content
{
    public class Design
    {
        public static ListItem[] AllSkins()
        {
            SkinContainerCollection AvailableSkins = SkinManager.Default.Skins;
            ListItem[] Result = new ListItem[AvailableSkins.Count];

            for (int i = 0; i < AvailableSkins.Count; i++)
            {
                Result[i] = new ListItem(AvailableSkins[i].SkinName);
            }

            return Result;
        }

        public static ListItem[] AllPaletts(string SkinName)
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

            ListItem[] Result = new ListItem[PaletteSet.Count];
            int StartIndex = 0;

            foreach (KeyValuePair<string, SkinSvgPalette> Pallet in PaletteSet)
            {
                Result[StartIndex] = new ListItem(Pallet.Key);
                StartIndex++;
            }

            return Result;
        }
    }
}

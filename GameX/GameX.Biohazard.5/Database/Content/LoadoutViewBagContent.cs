using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameX.Helpers;
using GameX.Modules;
using GameX.Database.ViewBag;

namespace GameX.Database.Content
{
    public class LoadoutViewBagContent
    {
        public static List<LoadoutViewBag> GetCollection()
        {
            try
            {
                string Dir = @"addons/GameX.Biohazard.5/prefabs/loadout/";

                if (!Directory.Exists(Dir))
                    Directory.CreateDirectory(Dir);

                DirectoryInfo Folder = new DirectoryInfo(Dir);
                FileInfo[] Files = Folder.GetFiles("*.json");
                List<LoadoutViewBag> Available = new List<LoadoutViewBag>();

                foreach (FileInfo file in Files)
                {
                    Available.Add(Serializer.Deserialize<LoadoutViewBag>(File.ReadAllText(Dir + file.Name)));
                }

                return Available.OrderBy(x => x.Name).ToList();
            }
            catch (Exception Ex)
            {
                Terminal.WriteLine(Ex);
                return new List<LoadoutViewBag>();
            }
        }
    }
}

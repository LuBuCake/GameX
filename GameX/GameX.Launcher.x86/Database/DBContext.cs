using System.Collections.Generic;
using GameX.Launcher.Database.Content;
using GameX.Launcher.Database.Type;

namespace GameX.Launcher.Database
{
    public class DB
    {
        public List<Addon> Addons { get; set; }
    }

    public static class DBContext
    {
        private static DB Database { get; set; }

        private static void BuildDatabase()
        {
            Database = new DB();
            Database.Addons = AddonContent.GetCollection();
        }

        public static DB GetDatabase()
        {
            if (Database == null)
                BuildDatabase();

            return Database;
        }
    }
}

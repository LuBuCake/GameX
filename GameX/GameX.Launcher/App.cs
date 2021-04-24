using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using GameX.Launcher.Base.Helpers;
using GameX.Launcher.Base.Types;

namespace GameX.Launcher
{
    public partial class App : XtraForm
    {
        // App Init //

        public App()
        {
            InitializeComponent();
        }

        private void App_Load(object sender, EventArgs e)
        {
            SetupControls();
        }

        private void SetupControls()
        {
            string[] Dirs = Directory.GetDirectories(Directory.GetCurrentDirectory(), "GameX.Biohazard.*");
            List<GameXInfo> Versions = new List<GameXInfo>();

            foreach (string Dir in Dirs)
            {
                if (File.Exists($"{Dir}/appinfo.json"))
                {
                    GameXInfo Info = Serializer.DeserializeGameXInfo(File.ReadAllText($"{Dir}/appinfo.json"));
                    Versions.Add(Info);
                }
            }

            GameXComboEdit.SelectedIndexChanged += GameX_IndexChanged;
            GameXComboEdit.Properties.Items.AddRange(Versions);
            GameXComboEdit.SelectedIndex = 0;

            GameXButton.Click += GameX_Click;
        }

        // Event Handlers //

        private void GameX_IndexChanged(object sender, EventArgs e)
        {
            ComboBoxEdit CBE = sender as ComboBoxEdit;
            GameXInfo Info = CBE.SelectedItem as GameXInfo;

            try
            {
                GameXPictureEdit.Image = Utility.GetImageFromStream(Info.GameXLogo);
            }
            catch (Exception)
            {
                //ignore
            }
        }

        private void GameX_Click(object sender, EventArgs e)
        {
            if (GameXComboEdit.Properties.Items.Count < 1)
            {
                DialogResult = DialogResult.OK;
                return;
            }

            GameXInfo Info = GameXComboEdit.SelectedItem as GameXInfo;
            Program.RuntimeDll = Info.GameXFile;
            DialogResult = DialogResult.OK;
        }
    }
}
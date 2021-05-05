
namespace GameX
{
    partial class App
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(App));
            this.MasterTabControl = new DevExpress.XtraTab.XtraTabControl();
            this.TabPageMain = new DevExpress.XtraTab.XtraTabPage();
            this.TabPageMainGPApp = new DevExpress.XtraEditors.GroupControl();
            this.AboutPictureEdit = new DevExpress.XtraEditors.PictureEdit();
            this.AppSettingsGP = new DevExpress.XtraEditors.GroupControl();
            this.MiscSettingsGP = new DevExpress.XtraEditors.GroupControl();
            this.UpdateModeComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.UpdateRateLabelControl = new DevExpress.XtraEditors.LabelControl();
            this.PaletteLabelControl = new DevExpress.XtraEditors.LabelControl();
            this.PaletteComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.LoadSaveGP = new DevExpress.XtraEditors.GroupControl();
            this.LoadSettingsButton = new DevExpress.XtraEditors.SimpleButton();
            this.SaveSettingsButton = new DevExpress.XtraEditors.SimpleButton();
            this.TabPageConsole = new DevExpress.XtraTab.XtraTabPage();
            this.ConsoleGP = new DevExpress.XtraEditors.GroupControl();
            this.ConsoleOutputMemoEdit = new DevExpress.XtraEditors.MemoEdit();
            this.ClearConsoleSimpleButton = new DevExpress.XtraEditors.SimpleButton();
            this.ConsoleModeComboBoxEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.ConsoleInputTextEdit = new DevExpress.XtraEditors.TextEdit();
            ((System.ComponentModel.ISupportInitialize)(this.MasterTabControl)).BeginInit();
            this.MasterTabControl.SuspendLayout();
            this.TabPageMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.TabPageMainGPApp)).BeginInit();
            this.TabPageMainGPApp.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AboutPictureEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AppSettingsGP)).BeginInit();
            this.AppSettingsGP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.MiscSettingsGP)).BeginInit();
            this.MiscSettingsGP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateModeComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaletteComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadSaveGP)).BeginInit();
            this.LoadSaveGP.SuspendLayout();
            this.TabPageConsole.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleGP)).BeginInit();
            this.ConsoleGP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleOutputMemoEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleModeComboBoxEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleInputTextEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // MasterTabControl
            // 
            this.MasterTabControl.HeaderAutoFill = DevExpress.Utils.DefaultBoolean.True;
            this.MasterTabControl.Location = new System.Drawing.Point(12, 12);
            this.MasterTabControl.Name = "MasterTabControl";
            this.MasterTabControl.SelectedTabPage = this.TabPageMain;
            this.MasterTabControl.Size = new System.Drawing.Size(640, 467);
            this.MasterTabControl.TabIndex = 19;
            this.MasterTabControl.TabPages.AddRange(new DevExpress.XtraTab.XtraTabPage[] {
            this.TabPageMain,
            this.TabPageConsole});
            // 
            // TabPageMain
            // 
            this.TabPageMain.Controls.Add(this.TabPageMainGPApp);
            this.TabPageMain.Name = "TabPageMain";
            this.TabPageMain.Size = new System.Drawing.Size(638, 438);
            this.TabPageMain.Text = "Main";
            // 
            // TabPageMainGPApp
            // 
            this.TabPageMainGPApp.AppearanceCaption.Options.UseTextOptions = true;
            this.TabPageMainGPApp.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.TabPageMainGPApp.Controls.Add(this.AboutPictureEdit);
            this.TabPageMainGPApp.Controls.Add(this.AppSettingsGP);
            this.TabPageMainGPApp.Location = new System.Drawing.Point(10, 10);
            this.TabPageMainGPApp.Margin = new System.Windows.Forms.Padding(10);
            this.TabPageMainGPApp.Name = "TabPageMainGPApp";
            this.TabPageMainGPApp.Size = new System.Drawing.Size(618, 418);
            this.TabPageMainGPApp.TabIndex = 0;
            // 
            // AboutPictureEdit
            // 
            this.AboutPictureEdit.Location = new System.Drawing.Point(315, 30);
            this.AboutPictureEdit.Name = "AboutPictureEdit";
            this.AboutPictureEdit.Properties.AllowFocused = false;
            this.AboutPictureEdit.Properties.PictureInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.AboutPictureEdit.Properties.ReadOnly = true;
            this.AboutPictureEdit.Properties.ShowMenu = false;
            this.AboutPictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.AboutPictureEdit.Size = new System.Drawing.Size(298, 69);
            this.AboutPictureEdit.TabIndex = 0;
            this.AboutPictureEdit.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            // 
            // AppSettingsGP
            // 
            this.AppSettingsGP.Controls.Add(this.MiscSettingsGP);
            this.AppSettingsGP.Controls.Add(this.LoadSaveGP);
            this.AppSettingsGP.Location = new System.Drawing.Point(315, 105);
            this.AppSettingsGP.Name = "AppSettingsGP";
            this.AppSettingsGP.Size = new System.Drawing.Size(298, 120);
            this.AppSettingsGP.TabIndex = 0;
            this.AppSettingsGP.Text = "App Settings";
            // 
            // MiscSettingsGP
            // 
            this.MiscSettingsGP.Controls.Add(this.UpdateModeComboBoxEdit);
            this.MiscSettingsGP.Controls.Add(this.UpdateRateLabelControl);
            this.MiscSettingsGP.Controls.Add(this.PaletteLabelControl);
            this.MiscSettingsGP.Controls.Add(this.PaletteComboBoxEdit);
            this.MiscSettingsGP.Location = new System.Drawing.Point(5, 30);
            this.MiscSettingsGP.Name = "MiscSettingsGP";
            this.MiscSettingsGP.Size = new System.Drawing.Size(170, 85);
            this.MiscSettingsGP.TabIndex = 0;
            // 
            // UpdateModeComboBoxEdit
            // 
            this.UpdateModeComboBoxEdit.Location = new System.Drawing.Point(52, 33);
            this.UpdateModeComboBoxEdit.Name = "UpdateModeComboBoxEdit";
            this.UpdateModeComboBoxEdit.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.UpdateModeComboBoxEdit.Properties.AllowFocused = false;
            this.UpdateModeComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.UpdateModeComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.UpdateModeComboBoxEdit.Size = new System.Drawing.Size(111, 20);
            this.UpdateModeComboBoxEdit.TabIndex = 0;
            this.UpdateModeComboBoxEdit.TabStop = false;
            this.UpdateModeComboBoxEdit.ToolTip = "Sets the update rate for the main loop system. Aways leave the Update Rate greate" +
    "r or equal to your game\'s FPS.";
            this.UpdateModeComboBoxEdit.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.UpdateModeComboBoxEdit.ToolTipTitle = "Update Rate";
            // 
            // UpdateRateLabelControl
            // 
            this.UpdateRateLabelControl.Location = new System.Drawing.Point(13, 36);
            this.UpdateRateLabelControl.Name = "UpdateRateLabelControl";
            this.UpdateRateLabelControl.Size = new System.Drawing.Size(26, 13);
            this.UpdateRateLabelControl.TabIndex = 0;
            this.UpdateRateLabelControl.Text = "Rate:";
            // 
            // PaletteLabelControl
            // 
            this.PaletteLabelControl.Location = new System.Drawing.Point(13, 62);
            this.PaletteLabelControl.Name = "PaletteLabelControl";
            this.PaletteLabelControl.Size = new System.Drawing.Size(25, 13);
            this.PaletteLabelControl.TabIndex = 0;
            this.PaletteLabelControl.Text = "Skin:";
            // 
            // PaletteComboBoxEdit
            // 
            this.PaletteComboBoxEdit.Location = new System.Drawing.Point(52, 59);
            this.PaletteComboBoxEdit.Name = "PaletteComboBoxEdit";
            this.PaletteComboBoxEdit.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.PaletteComboBoxEdit.Properties.AllowFocused = false;
            this.PaletteComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.PaletteComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.PaletteComboBoxEdit.Size = new System.Drawing.Size(111, 20);
            this.PaletteComboBoxEdit.TabIndex = 0;
            this.PaletteComboBoxEdit.TabStop = false;
            this.PaletteComboBoxEdit.ToolTip = "Changes the skin scheme for the application.";
            this.PaletteComboBoxEdit.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.PaletteComboBoxEdit.ToolTipTitle = "Skin";
            // 
            // LoadSaveGP
            // 
            this.LoadSaveGP.Controls.Add(this.LoadSettingsButton);
            this.LoadSaveGP.Controls.Add(this.SaveSettingsButton);
            this.LoadSaveGP.Location = new System.Drawing.Point(181, 30);
            this.LoadSaveGP.Name = "LoadSaveGP";
            this.LoadSaveGP.Size = new System.Drawing.Size(112, 85);
            this.LoadSaveGP.TabIndex = 0;
            // 
            // LoadSettingsButton
            // 
            this.LoadSettingsButton.AllowFocus = false;
            this.LoadSettingsButton.Location = new System.Drawing.Point(5, 57);
            this.LoadSettingsButton.Name = "LoadSettingsButton";
            this.LoadSettingsButton.Size = new System.Drawing.Size(102, 23);
            this.LoadSettingsButton.TabIndex = 1;
            this.LoadSettingsButton.TabStop = false;
            this.LoadSettingsButton.Text = "Load";
            this.LoadSettingsButton.ToolTip = "Loads the last saved configuration or the default ones.";
            this.LoadSettingsButton.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.LoadSettingsButton.ToolTipTitle = "Load";
            // 
            // SaveSettingsButton
            // 
            this.SaveSettingsButton.AllowFocus = false;
            this.SaveSettingsButton.ImageOptions.ImageToTextAlignment = DevExpress.XtraEditors.ImageAlignToText.LeftCenter;
            this.SaveSettingsButton.Location = new System.Drawing.Point(5, 30);
            this.SaveSettingsButton.Name = "SaveSettingsButton";
            this.SaveSettingsButton.Size = new System.Drawing.Size(102, 23);
            this.SaveSettingsButton.TabIndex = 0;
            this.SaveSettingsButton.TabStop = false;
            this.SaveSettingsButton.Text = "Save";
            this.SaveSettingsButton.ToolTip = "Saves the current configuration.";
            this.SaveSettingsButton.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.SaveSettingsButton.ToolTipTitle = "Save";
            // 
            // TabPageConsole
            // 
            this.TabPageConsole.Controls.Add(this.ConsoleGP);
            this.TabPageConsole.Name = "TabPageConsole";
            this.TabPageConsole.Size = new System.Drawing.Size(638, 438);
            this.TabPageConsole.Text = "Console";
            // 
            // ConsoleGP
            // 
            this.ConsoleGP.Controls.Add(this.ConsoleOutputMemoEdit);
            this.ConsoleGP.Controls.Add(this.ClearConsoleSimpleButton);
            this.ConsoleGP.Controls.Add(this.ConsoleModeComboBoxEdit);
            this.ConsoleGP.Controls.Add(this.ConsoleInputTextEdit);
            this.ConsoleGP.Location = new System.Drawing.Point(8, 10);
            this.ConsoleGP.Margin = new System.Windows.Forms.Padding(10);
            this.ConsoleGP.Name = "ConsoleGP";
            this.ConsoleGP.Size = new System.Drawing.Size(622, 418);
            this.ConsoleGP.TabIndex = 2;
            // 
            // ConsoleOutputMemoEdit
            // 
            this.ConsoleOutputMemoEdit.Location = new System.Drawing.Point(5, 30);
            this.ConsoleOutputMemoEdit.Name = "ConsoleOutputMemoEdit";
            this.ConsoleOutputMemoEdit.Properties.AllowFocused = false;
            this.ConsoleOutputMemoEdit.Properties.ReadOnly = true;
            this.ConsoleOutputMemoEdit.Properties.ShowNullValuePrompt = DevExpress.XtraEditors.ShowNullValuePromptOptions.NullValue;
            this.ConsoleOutputMemoEdit.Properties.UseReadOnlyAppearance = false;
            this.ConsoleOutputMemoEdit.Size = new System.Drawing.Size(612, 357);
            this.ConsoleOutputMemoEdit.TabIndex = 0;
            this.ConsoleOutputMemoEdit.TabStop = false;
            // 
            // ClearConsoleSimpleButton
            // 
            this.ClearConsoleSimpleButton.AllowFocus = false;
            this.ClearConsoleSimpleButton.Location = new System.Drawing.Point(465, 392);
            this.ClearConsoleSimpleButton.Name = "ClearConsoleSimpleButton";
            this.ClearConsoleSimpleButton.Size = new System.Drawing.Size(35, 20);
            this.ClearConsoleSimpleButton.TabIndex = 3;
            this.ClearConsoleSimpleButton.TabStop = false;
            this.ClearConsoleSimpleButton.Text = "Clear";
            this.ClearConsoleSimpleButton.ToolTip = "Clears the active interface\'s text.";
            this.ClearConsoleSimpleButton.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.ClearConsoleSimpleButton.ToolTipTitle = "Clear output";
            // 
            // ConsoleModeComboBoxEdit
            // 
            this.ConsoleModeComboBoxEdit.Location = new System.Drawing.Point(506, 392);
            this.ConsoleModeComboBoxEdit.Name = "ConsoleModeComboBoxEdit";
            this.ConsoleModeComboBoxEdit.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.ConsoleModeComboBoxEdit.Properties.AllowFocused = false;
            this.ConsoleModeComboBoxEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.ConsoleModeComboBoxEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.ConsoleModeComboBoxEdit.Size = new System.Drawing.Size(111, 20);
            this.ConsoleModeComboBoxEdit.TabIndex = 2;
            this.ConsoleModeComboBoxEdit.TabStop = false;
            this.ConsoleModeComboBoxEdit.ToolTip = "Changes the active interface, you can choose console for commands and logs or one" +
    " of the two available chats if connectd.";
            this.ConsoleModeComboBoxEdit.ToolTipIconType = DevExpress.Utils.ToolTipIconType.Information;
            this.ConsoleModeComboBoxEdit.ToolTipTitle = "Active interface";
            // 
            // ConsoleInputTextEdit
            // 
            this.ConsoleInputTextEdit.CausesValidation = false;
            this.ConsoleInputTextEdit.Location = new System.Drawing.Point(5, 392);
            this.ConsoleInputTextEdit.Name = "ConsoleInputTextEdit";
            this.ConsoleInputTextEdit.Properties.ValidateOnEnterKey = true;
            this.ConsoleInputTextEdit.Size = new System.Drawing.Size(454, 20);
            this.ConsoleInputTextEdit.TabIndex = 1;
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(664, 493);
            this.Controls.Add(this.MasterTabControl);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("App.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "App";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameX";
            this.Load += new System.EventHandler(this.App_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MasterTabControl)).EndInit();
            this.MasterTabControl.ResumeLayout(false);
            this.TabPageMain.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.TabPageMainGPApp)).EndInit();
            this.TabPageMainGPApp.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AboutPictureEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.AppSettingsGP)).EndInit();
            this.AppSettingsGP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.MiscSettingsGP)).EndInit();
            this.MiscSettingsGP.ResumeLayout(false);
            this.MiscSettingsGP.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UpdateModeComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PaletteComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.LoadSaveGP)).EndInit();
            this.LoadSaveGP.ResumeLayout(false);
            this.TabPageConsole.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleGP)).EndInit();
            this.ConsoleGP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleOutputMemoEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleModeComboBoxEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ConsoleInputTextEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraTab.XtraTabControl MasterTabControl;
        private DevExpress.XtraTab.XtraTabPage TabPageMain;
        private DevExpress.XtraEditors.GroupControl TabPageMainGPApp;
        private DevExpress.XtraEditors.PictureEdit AboutPictureEdit;
        private DevExpress.XtraEditors.GroupControl AppSettingsGP;
        private DevExpress.XtraEditors.GroupControl MiscSettingsGP;
        private DevExpress.XtraEditors.ComboBoxEdit UpdateModeComboBoxEdit;
        private DevExpress.XtraEditors.LabelControl UpdateRateLabelControl;
        private DevExpress.XtraEditors.LabelControl PaletteLabelControl;
        private DevExpress.XtraEditors.ComboBoxEdit PaletteComboBoxEdit;
        private DevExpress.XtraEditors.GroupControl LoadSaveGP;
        private DevExpress.XtraEditors.SimpleButton LoadSettingsButton;
        private DevExpress.XtraEditors.SimpleButton SaveSettingsButton;
        private DevExpress.XtraTab.XtraTabPage TabPageConsole;
        private DevExpress.XtraEditors.GroupControl ConsoleGP;
        public DevExpress.XtraEditors.MemoEdit ConsoleOutputMemoEdit;
        public DevExpress.XtraEditors.SimpleButton ClearConsoleSimpleButton;
        private DevExpress.XtraEditors.ComboBoxEdit ConsoleModeComboBoxEdit;
        public DevExpress.XtraEditors.TextEdit ConsoleInputTextEdit;
    }
}


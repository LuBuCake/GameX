
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
            this.tabPane1 = new DevExpress.XtraBars.Navigation.TabPane();
            this.CharPage = new DevExpress.XtraBars.Navigation.TabNavigationPage();
            this.P1CosComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.P1FreezeCharCosButton = new DevExpress.XtraEditors.CheckButton();
            this.P1CharComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.P1CharPictureBox = new DevExpress.XtraEditors.PictureEdit();
            this.P2CosComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.P2FreezeCharCosButton = new DevExpress.XtraEditors.CheckButton();
            this.P2CharComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.P2CharPictureBox = new DevExpress.XtraEditors.PictureEdit();
            this.P3CosComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.P3FreezeCharCosButton = new DevExpress.XtraEditors.CheckButton();
            this.P3CharComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.P3CharPictureBox = new DevExpress.XtraEditors.PictureEdit();
            this.P4CosComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            this.P4FreezeCharCosButton = new DevExpress.XtraEditors.CheckButton();
            this.P4CharPictureBox = new DevExpress.XtraEditors.PictureEdit();
            this.P4CharComboBox = new DevExpress.XtraEditors.ComboBoxEdit();
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).BeginInit();
            this.tabPane1.SuspendLayout();
            this.CharPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.P1CosComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P1CharComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P1CharPictureBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P2CosComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P2CharComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P2CharPictureBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P3CosComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P3CharComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P3CharPictureBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4CosComboBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4CharPictureBox.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4CharComboBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // tabPane1
            // 
            this.tabPane1.Controls.Add(this.CharPage);
            this.tabPane1.Location = new System.Drawing.Point(12, 12);
            this.tabPane1.Name = "tabPane1";
            this.tabPane1.Pages.AddRange(new DevExpress.XtraBars.Navigation.NavigationPageBase[] {
            this.CharPage});
            this.tabPane1.RegularSize = new System.Drawing.Size(664, 393);
            this.tabPane1.SelectedPage = this.CharPage;
            this.tabPane1.Size = new System.Drawing.Size(664, 393);
            this.tabPane1.TabIndex = 0;
            this.tabPane1.Text = "tabPane1";
            // 
            // CharPage
            // 
            this.CharPage.Caption = "Character";
            this.CharPage.Controls.Add(this.P4CharComboBox);
            this.CharPage.Controls.Add(this.P4CosComboBox);
            this.CharPage.Controls.Add(this.P4FreezeCharCosButton);
            this.CharPage.Controls.Add(this.P4CharPictureBox);
            this.CharPage.Controls.Add(this.P3CosComboBox);
            this.CharPage.Controls.Add(this.P3FreezeCharCosButton);
            this.CharPage.Controls.Add(this.P3CharComboBox);
            this.CharPage.Controls.Add(this.P3CharPictureBox);
            this.CharPage.Controls.Add(this.P2CosComboBox);
            this.CharPage.Controls.Add(this.P2FreezeCharCosButton);
            this.CharPage.Controls.Add(this.P2CharComboBox);
            this.CharPage.Controls.Add(this.P2CharPictureBox);
            this.CharPage.Controls.Add(this.P1CosComboBox);
            this.CharPage.Controls.Add(this.P1FreezeCharCosButton);
            this.CharPage.Controls.Add(this.P1CharComboBox);
            this.CharPage.Controls.Add(this.P1CharPictureBox);
            this.CharPage.Name = "CharPage";
            this.CharPage.Size = new System.Drawing.Size(664, 364);
            this.CharPage.ToolTip = "Change characters and costumes.";
            // 
            // P1CosComboBox
            // 
            this.P1CosComboBox.Location = new System.Drawing.Point(24, 178);
            this.P1CosComboBox.Name = "P1CosComboBox";
            this.P1CosComboBox.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.P1CosComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.P1CosComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.P1CosComboBox.Size = new System.Drawing.Size(149, 20);
            this.P1CosComboBox.TabIndex = 5;
            // 
            // P1FreezeCharCosButton
            // 
            this.P1FreezeCharCosButton.Location = new System.Drawing.Point(120, 150);
            this.P1FreezeCharCosButton.Name = "P1FreezeCharCosButton";
            this.P1FreezeCharCosButton.Size = new System.Drawing.Size(53, 23);
            this.P1FreezeCharCosButton.TabIndex = 4;
            this.P1FreezeCharCosButton.Text = "Frozen";
            // 
            // P1CharComboBox
            // 
            this.P1CharComboBox.Location = new System.Drawing.Point(24, 152);
            this.P1CharComboBox.Name = "P1CharComboBox";
            this.P1CharComboBox.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.P1CharComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.P1CharComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.P1CharComboBox.Size = new System.Drawing.Size(90, 20);
            this.P1CharComboBox.TabIndex = 2;
            // 
            // P1CharPictureBox
            // 
            this.P1CharPictureBox.Location = new System.Drawing.Point(24, 31);
            this.P1CharPictureBox.Name = "P1CharPictureBox";
            this.P1CharPictureBox.Properties.PictureAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.P1CharPictureBox.Properties.ReadOnly = true;
            this.P1CharPictureBox.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.P1CharPictureBox.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.P1CharPictureBox.Size = new System.Drawing.Size(149, 115);
            this.P1CharPictureBox.TabIndex = 0;
            this.P1CharPictureBox.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            // 
            // P2CosComboBox
            // 
            this.P2CosComboBox.Location = new System.Drawing.Point(179, 178);
            this.P2CosComboBox.Name = "P2CosComboBox";
            this.P2CosComboBox.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.P2CosComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.P2CosComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.P2CosComboBox.Size = new System.Drawing.Size(149, 20);
            this.P2CosComboBox.TabIndex = 9;
            // 
            // P2FreezeCharCosButton
            // 
            this.P2FreezeCharCosButton.Location = new System.Drawing.Point(275, 150);
            this.P2FreezeCharCosButton.Name = "P2FreezeCharCosButton";
            this.P2FreezeCharCosButton.Size = new System.Drawing.Size(53, 23);
            this.P2FreezeCharCosButton.TabIndex = 8;
            this.P2FreezeCharCosButton.Text = "Frozen";
            // 
            // P2CharComboBox
            // 
            this.P2CharComboBox.Location = new System.Drawing.Point(179, 152);
            this.P2CharComboBox.Name = "P2CharComboBox";
            this.P2CharComboBox.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.P2CharComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.P2CharComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.P2CharComboBox.Size = new System.Drawing.Size(90, 20);
            this.P2CharComboBox.TabIndex = 7;
            // 
            // P2CharPictureBox
            // 
            this.P2CharPictureBox.Location = new System.Drawing.Point(179, 31);
            this.P2CharPictureBox.Name = "P2CharPictureBox";
            this.P2CharPictureBox.Properties.PictureAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.P2CharPictureBox.Properties.ReadOnly = true;
            this.P2CharPictureBox.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.P2CharPictureBox.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.P2CharPictureBox.Size = new System.Drawing.Size(149, 115);
            this.P2CharPictureBox.TabIndex = 6;
            this.P2CharPictureBox.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            // 
            // P3CosComboBox
            // 
            this.P3CosComboBox.Location = new System.Drawing.Point(334, 178);
            this.P3CosComboBox.Name = "P3CosComboBox";
            this.P3CosComboBox.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.P3CosComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.P3CosComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.P3CosComboBox.Size = new System.Drawing.Size(149, 20);
            this.P3CosComboBox.TabIndex = 13;
            // 
            // P3FreezeCharCosButton
            // 
            this.P3FreezeCharCosButton.Location = new System.Drawing.Point(430, 150);
            this.P3FreezeCharCosButton.Name = "P3FreezeCharCosButton";
            this.P3FreezeCharCosButton.Size = new System.Drawing.Size(53, 23);
            this.P3FreezeCharCosButton.TabIndex = 12;
            this.P3FreezeCharCosButton.Text = "Frozen";
            // 
            // P3CharComboBox
            // 
            this.P3CharComboBox.Location = new System.Drawing.Point(334, 152);
            this.P3CharComboBox.Name = "P3CharComboBox";
            this.P3CharComboBox.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.P3CharComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.P3CharComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.P3CharComboBox.Size = new System.Drawing.Size(90, 20);
            this.P3CharComboBox.TabIndex = 11;
            // 
            // P3CharPictureBox
            // 
            this.P3CharPictureBox.Location = new System.Drawing.Point(334, 31);
            this.P3CharPictureBox.Name = "P3CharPictureBox";
            this.P3CharPictureBox.Properties.PictureAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.P3CharPictureBox.Properties.ReadOnly = true;
            this.P3CharPictureBox.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.P3CharPictureBox.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.P3CharPictureBox.Size = new System.Drawing.Size(149, 115);
            this.P3CharPictureBox.TabIndex = 10;
            this.P3CharPictureBox.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            // 
            // P4CosComboBox
            // 
            this.P4CosComboBox.Location = new System.Drawing.Point(489, 178);
            this.P4CosComboBox.Name = "P4CosComboBox";
            this.P4CosComboBox.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.P4CosComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.P4CosComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.P4CosComboBox.Size = new System.Drawing.Size(149, 20);
            this.P4CosComboBox.TabIndex = 16;
            // 
            // P4FreezeCharCosButton
            // 
            this.P4FreezeCharCosButton.Location = new System.Drawing.Point(586, 150);
            this.P4FreezeCharCosButton.Name = "P4FreezeCharCosButton";
            this.P4FreezeCharCosButton.Size = new System.Drawing.Size(53, 23);
            this.P4FreezeCharCosButton.TabIndex = 15;
            this.P4FreezeCharCosButton.Text = "Frozen";
            // 
            // P4CharPictureBox
            // 
            this.P4CharPictureBox.Location = new System.Drawing.Point(489, 31);
            this.P4CharPictureBox.Name = "P4CharPictureBox";
            this.P4CharPictureBox.Properties.PictureAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.P4CharPictureBox.Properties.ReadOnly = true;
            this.P4CharPictureBox.Properties.ShowCameraMenuItem = DevExpress.XtraEditors.Controls.CameraMenuItemVisibility.Auto;
            this.P4CharPictureBox.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Squeeze;
            this.P4CharPictureBox.Size = new System.Drawing.Size(149, 115);
            this.P4CharPictureBox.TabIndex = 14;
            this.P4CharPictureBox.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            // 
            // P4CharComboBox
            // 
            this.P4CharComboBox.Location = new System.Drawing.Point(489, 153);
            this.P4CharComboBox.Name = "P4CharComboBox";
            this.P4CharComboBox.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.P4CharComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.P4CharComboBox.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.P4CharComboBox.Size = new System.Drawing.Size(90, 20);
            this.P4CharComboBox.TabIndex = 17;
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 417);
            this.Controls.Add(this.tabPane1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("App.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "App";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameX";
            this.Load += new System.EventHandler(this.App_Load);
            ((System.ComponentModel.ISupportInitialize)(this.tabPane1)).EndInit();
            this.tabPane1.ResumeLayout(false);
            this.CharPage.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.P1CosComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P1CharComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P1CharPictureBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P2CosComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P2CharComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P2CharPictureBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P3CosComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P3CharComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P3CharPictureBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4CosComboBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4CharPictureBox.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.P4CharComboBox.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraBars.Navigation.TabPane tabPane1;
        private DevExpress.XtraBars.Navigation.TabNavigationPage CharPage;
        private DevExpress.XtraEditors.CheckButton P1FreezeCharCosButton;
        private DevExpress.XtraEditors.ComboBoxEdit P1CharComboBox;
        private DevExpress.XtraEditors.PictureEdit P1CharPictureBox;
        private DevExpress.XtraEditors.ComboBoxEdit P1CosComboBox;
        private DevExpress.XtraEditors.ComboBoxEdit P2CosComboBox;
        private DevExpress.XtraEditors.CheckButton P2FreezeCharCosButton;
        private DevExpress.XtraEditors.ComboBoxEdit P2CharComboBox;
        private DevExpress.XtraEditors.PictureEdit P2CharPictureBox;
        private DevExpress.XtraEditors.ComboBoxEdit P4CharComboBox;
        private DevExpress.XtraEditors.ComboBoxEdit P4CosComboBox;
        private DevExpress.XtraEditors.CheckButton P4FreezeCharCosButton;
        private DevExpress.XtraEditors.PictureEdit P4CharPictureBox;
        private DevExpress.XtraEditors.ComboBoxEdit P3CosComboBox;
        private DevExpress.XtraEditors.CheckButton P3FreezeCharCosButton;
        private DevExpress.XtraEditors.ComboBoxEdit P3CharComboBox;
        private DevExpress.XtraEditors.PictureEdit P3CharPictureBox;
    }
}


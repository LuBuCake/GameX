
namespace GameX.Launcher
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
            this.SelectorGP = new DevExpress.XtraEditors.GroupControl();
            this.GameXButton = new DevExpress.XtraEditors.SimpleButton();
            this.GameXComboEdit = new DevExpress.XtraEditors.ComboBoxEdit();
            this.GameXPictureEdit = new DevExpress.XtraEditors.PictureEdit();
            ((System.ComponentModel.ISupportInitialize)(this.SelectorGP)).BeginInit();
            this.SelectorGP.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.GameXComboEdit.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.GameXPictureEdit.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // SelectorGP
            // 
            this.SelectorGP.AppearanceCaption.Options.UseTextOptions = true;
            this.SelectorGP.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.SelectorGP.Controls.Add(this.GameXButton);
            this.SelectorGP.Controls.Add(this.GameXComboEdit);
            this.SelectorGP.Controls.Add(this.GameXPictureEdit);
            this.SelectorGP.Location = new System.Drawing.Point(12, 12);
            this.SelectorGP.Name = "SelectorGP";
            this.SelectorGP.Size = new System.Drawing.Size(308, 133);
            this.SelectorGP.TabIndex = 0;
            this.SelectorGP.Text = "Available Versions";
            // 
            // GameXButton
            // 
            this.GameXButton.AllowFocus = false;
            this.GameXButton.Location = new System.Drawing.Point(231, 105);
            this.GameXButton.Name = "GameXButton";
            this.GameXButton.Size = new System.Drawing.Size(72, 20);
            this.GameXButton.TabIndex = 3;
            this.GameXButton.Text = "Launch";
            // 
            // GameXComboEdit
            // 
            this.GameXComboEdit.Location = new System.Drawing.Point(5, 105);
            this.GameXComboEdit.Name = "GameXComboEdit";
            this.GameXComboEdit.Properties.AllowDropDownWhenReadOnly = DevExpress.Utils.DefaultBoolean.True;
            this.GameXComboEdit.Properties.AllowFocused = false;
            this.GameXComboEdit.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.GameXComboEdit.Properties.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            this.GameXComboEdit.Size = new System.Drawing.Size(220, 20);
            this.GameXComboEdit.TabIndex = 2;
            this.GameXComboEdit.TabStop = false;
            // 
            // GameXPictureEdit
            // 
            this.GameXPictureEdit.Location = new System.Drawing.Point(5, 30);
            this.GameXPictureEdit.Name = "GameXPictureEdit";
            this.GameXPictureEdit.Properties.AllowFocused = false;
            this.GameXPictureEdit.Properties.PictureInterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
            this.GameXPictureEdit.Properties.ReadOnly = true;
            this.GameXPictureEdit.Properties.ShowMenu = false;
            this.GameXPictureEdit.Properties.SizeMode = DevExpress.XtraEditors.Controls.PictureSizeMode.Stretch;
            this.GameXPictureEdit.Size = new System.Drawing.Size(298, 69);
            this.GameXPictureEdit.TabIndex = 1;
            this.GameXPictureEdit.UseDirectXPaint = DevExpress.Utils.DefaultBoolean.True;
            // 
            // App
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 161);
            this.Controls.Add(this.SelectorGP);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.IconOptions.Image = ((System.Drawing.Image)(resources.GetObject("App.IconOptions.Image")));
            this.MaximizeBox = false;
            this.Name = "App";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GameX - Launcher";
            this.Load += new System.EventHandler(this.App_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SelectorGP)).EndInit();
            this.SelectorGP.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.GameXComboEdit.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.GameXPictureEdit.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.GroupControl SelectorGP;
        private DevExpress.XtraEditors.PictureEdit GameXPictureEdit;
        private DevExpress.XtraEditors.SimpleButton GameXButton;
        public DevExpress.XtraEditors.ComboBoxEdit GameXComboEdit;
    }
}


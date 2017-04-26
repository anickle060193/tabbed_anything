using System.Windows.Forms;

namespace TabbedAnything
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose( bool disposing )
        {
            if( disposing && ( components != null ) )
            {
                components.Dispose();
            }
            base.Dispose( disposing );
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.OK = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.ConfirmOnCloseCheck = new System.Windows.Forms.CheckBox();
            this.SettingsToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.SettingsTabs = new System.Windows.Forms.TabControl();
            this.KeyboardShortcutsPage = new System.Windows.Forms.TabPage();
            this.KeyBoardShortcutsTableLayout = new System.Windows.Forms.TableLayoutPanel();
            this.OtherSettingsTab = new System.Windows.Forms.TabPage();
            this.CloseWindowOnLastTabClosedCheck = new System.Windows.Forms.CheckBox();
            this.CloseToSystemTrayCheck = new System.Windows.Forms.CheckBox();
            this.ModifiedTabFontDialog = new System.Windows.Forms.FontDialog();
            this.NormalTabFontDialog = new System.Windows.Forms.FontDialog();
            this.SettingsTabs.SuspendLayout();
            this.KeyboardShortcutsPage.SuspendLayout();
            this.OtherSettingsTab.SuspendLayout();
            this.SuspendLayout();
            // 
            // OK
            // 
            this.OK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.OK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.OK.Location = new System.Drawing.Point(316, 326);
            this.OK.Name = "OK";
            this.OK.Size = new System.Drawing.Size(75, 23);
            this.OK.TabIndex = 3;
            this.OK.Text = "OK";
            this.OK.UseVisualStyleBackColor = true;
            // 
            // Cancel
            // 
            this.Cancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.Cancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Cancel.Location = new System.Drawing.Point(397, 326);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 4;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            // 
            // ConfirmOnCloseCheck
            // 
            this.ConfirmOnCloseCheck.AutoSize = true;
            this.ConfirmOnCloseCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ConfirmOnCloseCheck.Location = new System.Drawing.Point(6, 6);
            this.ConfirmOnCloseCheck.Name = "ConfirmOnCloseCheck";
            this.ConfirmOnCloseCheck.Size = new System.Drawing.Size(133, 17);
            this.ConfirmOnCloseCheck.TabIndex = 10;
            this.ConfirmOnCloseCheck.Text = "Prompt Before Closing:";
            this.ConfirmOnCloseCheck.UseVisualStyleBackColor = true;
            // 
            // SettingsTabs
            // 
            this.SettingsTabs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SettingsTabs.Controls.Add(this.KeyboardShortcutsPage);
            this.SettingsTabs.Controls.Add(this.OtherSettingsTab);
            this.SettingsTabs.Location = new System.Drawing.Point(12, 12);
            this.SettingsTabs.Name = "SettingsTabs";
            this.SettingsTabs.SelectedIndex = 0;
            this.SettingsTabs.Size = new System.Drawing.Size(460, 308);
            this.SettingsTabs.TabIndex = 12;
            // 
            // KeyboardShortcutsPage
            // 
            this.KeyboardShortcutsPage.AutoScroll = true;
            this.KeyboardShortcutsPage.Controls.Add(this.KeyBoardShortcutsTableLayout);
            this.KeyboardShortcutsPage.Location = new System.Drawing.Point(4, 22);
            this.KeyboardShortcutsPage.Name = "KeyboardShortcutsPage";
            this.KeyboardShortcutsPage.Padding = new System.Windows.Forms.Padding(3);
            this.KeyboardShortcutsPage.Size = new System.Drawing.Size(452, 282);
            this.KeyboardShortcutsPage.TabIndex = 5;
            this.KeyboardShortcutsPage.Text = "Keyboard Shortcuts";
            this.KeyboardShortcutsPage.UseVisualStyleBackColor = true;
            // 
            // KeyBoardShortcutsTableLayout
            // 
            this.KeyBoardShortcutsTableLayout.AutoSize = true;
            this.KeyBoardShortcutsTableLayout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.KeyBoardShortcutsTableLayout.ColumnCount = 2;
            this.KeyBoardShortcutsTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.KeyBoardShortcutsTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.KeyBoardShortcutsTableLayout.Dock = System.Windows.Forms.DockStyle.Top;
            this.KeyBoardShortcutsTableLayout.Location = new System.Drawing.Point(3, 3);
            this.KeyBoardShortcutsTableLayout.Name = "KeyBoardShortcutsTableLayout";
            this.KeyBoardShortcutsTableLayout.RowCount = 1;
            this.KeyBoardShortcutsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.KeyBoardShortcutsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.KeyBoardShortcutsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.KeyBoardShortcutsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.KeyBoardShortcutsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.KeyBoardShortcutsTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 1F));
            this.KeyBoardShortcutsTableLayout.Size = new System.Drawing.Size(446, 0);
            this.KeyBoardShortcutsTableLayout.TabIndex = 2;
            // 
            // OtherSettingsTab
            // 
            this.OtherSettingsTab.Controls.Add(this.CloseWindowOnLastTabClosedCheck);
            this.OtherSettingsTab.Controls.Add(this.CloseToSystemTrayCheck);
            this.OtherSettingsTab.Controls.Add(this.ConfirmOnCloseCheck);
            this.OtherSettingsTab.Location = new System.Drawing.Point(4, 22);
            this.OtherSettingsTab.Name = "OtherSettingsTab";
            this.OtherSettingsTab.Padding = new System.Windows.Forms.Padding(3);
            this.OtherSettingsTab.Size = new System.Drawing.Size(452, 282);
            this.OtherSettingsTab.TabIndex = 2;
            this.OtherSettingsTab.Text = "Other";
            this.OtherSettingsTab.UseVisualStyleBackColor = true;
            // 
            // CloseWindowOnLastTabClosedCheck
            // 
            this.CloseWindowOnLastTabClosedCheck.AutoSize = true;
            this.CloseWindowOnLastTabClosedCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CloseWindowOnLastTabClosedCheck.Location = new System.Drawing.Point(6, 29);
            this.CloseWindowOnLastTabClosedCheck.Name = "CloseWindowOnLastTabClosedCheck";
            this.CloseWindowOnLastTabClosedCheck.Size = new System.Drawing.Size(189, 17);
            this.CloseWindowOnLastTabClosedCheck.TabIndex = 13;
            this.CloseWindowOnLastTabClosedCheck.Text = "Close Window on Last Tab Closed";
            this.CloseWindowOnLastTabClosedCheck.UseVisualStyleBackColor = true;
            // 
            // CloseToSystemTrayCheck
            // 
            this.CloseToSystemTrayCheck.AutoSize = true;
            this.CloseToSystemTrayCheck.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.CloseToSystemTrayCheck.Location = new System.Drawing.Point(6, 52);
            this.CloseToSystemTrayCheck.Name = "CloseToSystemTrayCheck";
            this.CloseToSystemTrayCheck.Size = new System.Drawing.Size(125, 17);
            this.CloseToSystemTrayCheck.TabIndex = 12;
            this.CloseToSystemTrayCheck.Text = "Close to System Tray";
            this.CloseToSystemTrayCheck.UseVisualStyleBackColor = true;
            // 
            // ModifiedTabFontDialog
            // 
            this.ModifiedTabFontDialog.AllowScriptChange = false;
            this.ModifiedTabFontDialog.FontMustExist = true;
            this.ModifiedTabFontDialog.ShowColor = true;
            // 
            // NormalTabFontDialog
            // 
            this.NormalTabFontDialog.AllowScriptChange = false;
            this.NormalTabFontDialog.FontMustExist = true;
            this.NormalTabFontDialog.ShowColor = true;
            // 
            // SettingsForm
            // 
            this.AcceptButton = this.OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.Cancel;
            this.ClientSize = new System.Drawing.Size(484, 361);
            this.Controls.Add(this.SettingsTabs);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.OK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 400);
            this.Name = "SettingsForm";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.SettingsTabs.ResumeLayout(false);
            this.KeyboardShortcutsPage.ResumeLayout(false);
            this.KeyboardShortcutsPage.PerformLayout();
            this.OtherSettingsTab.ResumeLayout(false);
            this.OtherSettingsTab.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private Button OK;
        private Button Cancel;
        private CheckBox ConfirmOnCloseCheck;
        private ToolTip SettingsToolTip;
        private TabControl SettingsTabs;
        private TabPage OtherSettingsTab;
        private CheckBox CloseToSystemTrayCheck;
        private FontDialog ModifiedTabFontDialog;
        private CheckBox CloseWindowOnLastTabClosedCheck;
        private TabPage KeyboardShortcutsPage;
        private TableLayoutPanel KeyBoardShortcutsTableLayout;
        private FontDialog NormalTabFontDialog;
    }
}
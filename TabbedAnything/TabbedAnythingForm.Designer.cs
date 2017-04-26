namespace TabbedAnything
{
    partial class TabbedAnythingForm
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
            this.NewTabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.TabContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CloseRepoTabMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.OpenRepoMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OptionsMenuSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.SettingsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowDebugLogMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ExitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.LogTabs = new Tabs.TabControl();
            this.TabContextMenu.SuspendLayout();
            this.OptionsContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // NewTabContextMenu
            // 
            this.NewTabContextMenu.Name = "NewTabContextMenu";
            this.NewTabContextMenu.Size = new System.Drawing.Size(61, 4);
            // 
            // TabContextMenu
            // 
            this.TabContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CloseRepoTabMenuItem});
            this.TabContextMenu.Name = "TabContextMenu";
            this.TabContextMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.TabContextMenu.Size = new System.Drawing.Size(104, 26);
            // 
            // CloseRepoTabMenuItem
            // 
            this.CloseRepoTabMenuItem.Name = "CloseRepoTabMenuItem";
            this.CloseRepoTabMenuItem.Size = new System.Drawing.Size(103, 22);
            this.CloseRepoTabMenuItem.Text = "Close";
            // 
            // OptionsContextMenu
            // 
            this.OptionsContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenRepoMenuItem,
            this.OptionsMenuSeparator,
            this.SettingsMenuItem,
            this.ShowDebugLogMenuItem,
            this.AboutMenuItem,
            this.ExitMenuItem});
            this.OptionsContextMenu.Name = "OptionsContextMenu";
            this.OptionsContextMenu.Size = new System.Drawing.Size(165, 120);
            // 
            // OpenRepoMenuItem
            // 
            this.OpenRepoMenuItem.Name = "OpenRepoMenuItem";
            this.OpenRepoMenuItem.Size = new System.Drawing.Size(164, 22);
            this.OpenRepoMenuItem.Text = "Open Repo";
            // 
            // OptionsMenuSeparator
            // 
            this.OptionsMenuSeparator.Name = "OptionsMenuSeparator";
            this.OptionsMenuSeparator.Size = new System.Drawing.Size(161, 6);
            // 
            // SettingsMenuItem
            // 
            this.SettingsMenuItem.Name = "SettingsMenuItem";
            this.SettingsMenuItem.Size = new System.Drawing.Size(164, 22);
            this.SettingsMenuItem.Text = "Settings";
            // 
            // ShowDebugLogMenuItem
            // 
            this.ShowDebugLogMenuItem.Name = "ShowDebugLogMenuItem";
            this.ShowDebugLogMenuItem.Size = new System.Drawing.Size(164, 22);
            this.ShowDebugLogMenuItem.Text = "Show Debug Log";
            // 
            // AboutMenuItem
            // 
            this.AboutMenuItem.Name = "AboutMenuItem";
            this.AboutMenuItem.Size = new System.Drawing.Size(164, 22);
            this.AboutMenuItem.Text = "About";
            // 
            // ExitMenuItem
            // 
            this.ExitMenuItem.Name = "ExitMenuItem";
            this.ExitMenuItem.Size = new System.Drawing.Size(164, 22);
            this.ExitMenuItem.Text = "Exit";
            // 
            // LogTabs
            // 
            this.LogTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LogTabs.Location = new System.Drawing.Point(0, 0);
            this.LogTabs.Name = "LogTabs";
            this.LogTabs.NewTabContextMenu = this.NewTabContextMenu;
            this.LogTabs.OptionsMenu = this.OptionsContextMenu;
            this.LogTabs.Size = new System.Drawing.Size(644, 486);
            this.LogTabs.TabContextMenu = this.TabContextMenu;
            this.LogTabs.TabIndex = 3;
            // 
            // TabbedAnythingForm
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(644, 486);
            this.Controls.Add(this.LogTabs);
            this.MinimumSize = new System.Drawing.Size(660, 525);
            this.Name = "TabbedAnythingForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Tabbed Anything";
            this.TabContextMenu.ResumeLayout(false);
            this.OptionsContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ContextMenuStrip NewTabContextMenu;
        private System.Windows.Forms.ContextMenuStrip TabContextMenu;
        private System.Windows.Forms.ToolStripMenuItem CloseRepoTabMenuItem;
        private Tabs.TabControl LogTabs;
        private System.Windows.Forms.ContextMenuStrip OptionsContextMenu;
        private System.Windows.Forms.ToolStripMenuItem OpenRepoMenuItem;
        private System.Windows.Forms.ToolStripSeparator OptionsMenuSeparator;
        private System.Windows.Forms.ToolStripMenuItem SettingsMenuItem;
        private System.Windows.Forms.ToolStripMenuItem AboutMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ExitMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ShowDebugLogMenuItem;
    }
}


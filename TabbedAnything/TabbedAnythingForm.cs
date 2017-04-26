using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TabbedAnything.Properties;
using System.Configuration;
using Microsoft.WindowsAPICodePack.Dialogs;
using Newtonsoft.Json;
using log4net;
using log4net.Config;
using Tabs;
using System.IO;
using System.Collections.Immutable;

namespace TabbedAnything
{
    public partial class TabbedAnythingForm : Form
    {
        private static readonly ILog LOG = LogManager.GetLogger( typeof( TabbedAnythingForm ) );

        private static readonly Keys[] FORWARD_KEYS = new[]
        {
            Keys.F5,
            Keys.Up,
            Keys.Down
        };

        private readonly Dictionary<int, TabControllerTag> _tags = new Dictionary<int, TabControllerTag>();
        private readonly CommonOpenFileDialog _folderDialog;
        private readonly Point _createdAtPoint;
        private readonly Stack<String> _closedRepos = new Stack<String>();

        public TabbedAnythingForm( Point createdAtPoint )
        {
            LOG.Debug( "Constructor" );

            InitializeComponent();
            InitializeEventHandlers();

            _createdAtPoint = createdAtPoint;

            _folderDialog = new CommonOpenFileDialog();
            _folderDialog.IsFolderPicker = true;

            UpdateFromSettings( true );
        }

        protected override void OnHandleCreated( EventArgs e )
        {
            base.OnHandleCreated( e );

            KeyboardShortcutsManager.Instance.AddHandle( this.Handle );
        }

        protected override void DestroyHandle()
        {
            KeyboardShortcutsManager.Instance.RemoveHandle( this.Handle );

            base.DestroyHandle();
        }

        protected override bool ProcessCmdKey( ref Message msg, Keys keyData )
        {
            if( FORWARD_KEYS.Any( k => keyData == k ) )
            {
                TabControllerTag tag = LogTabs.SelectedTab?.Controller();
                if( tag != null )
                {
                    Native.SetForegroundWindow( tag.Process.MainWindowHandle );
                    Native.SendKeyDown( tag.Process.MainWindowHandle, keyData );
                    return true;
                }
            }

            return base.ProcessCmdKey( ref msg, keyData );
        }

        private void UpdateFromSettings( bool updateWindowState )
        {
            if( updateWindowState )
            {
                if( !Settings.Default.Size.IsEmpty )
                {
                    this.Size = Settings.Default.Size;
                    this.CenterToScreen();
                }

                if( !_createdAtPoint.IsEmpty )
                {
                    this.StartPosition = FormStartPosition.Manual;

                    Rectangle formBounds = this.Bounds;
                    Rectangle tabBounds = LogTabs.RectangleToScreen( LogTabs.GetTabRect( 0 ) );
                    int tabX = tabBounds.Left - formBounds.Left;
                    int tabY = tabBounds.Top - formBounds.Top;

                    int x = _createdAtPoint.X - tabX - tabBounds.Width / 2;
                    int y = _createdAtPoint.Y - tabY - tabBounds.Height / 2;
                    this.Location = new Point( x, y );
                }

                if( Settings.Default.Maximized )
                {
                    this.WindowState = FormWindowState.Maximized;
                }
            }

            lock( _tags )
            {
                foreach( TabControllerTag tag in _tags.Values )
                {
                    tag.UpdateTabDisplay();
                    tag.UpdateIcon();
                }
            }
        }

        private async Task OpenLog( String path, IEnumerable<String> references = null )
        {
            Process p = GitAction.Log( path, references );
            await AddNewLogProcess( p, path );
        }

        internal async void HandleKeyboardShortcut( KeyboardShortcuts keyboardShortcut )
        {
            LOG.DebugFormat( "HandleKeyboardShortcut - KeyboardShortcut: {0}", keyboardShortcut );

            if( keyboardShortcut == KeyboardShortcuts.NewTab )
            {
                await FindRepo();
            }
            else if( keyboardShortcut == KeyboardShortcuts.NextTab )
            {
                LogTabs.NextTab();
            }
            else if( keyboardShortcut == KeyboardShortcuts.PreviousTab )
            {
                LogTabs.PreviousTab();
            }
            else if( keyboardShortcut == KeyboardShortcuts.CloseTab )
            {
                CloseTab( LogTabs.SelectedTab );
            }
            else if( keyboardShortcut == KeyboardShortcuts.ReopenClosedTab )
            {
                if( _closedRepos.Count > 0 )
                {
                    String repo = _closedRepos.Pop();

                    LOG.DebugFormat( "HotKey - Reopen Closed Tab - Reopening: {0}", repo );

                    await OpenLog( repo );
                }
            }
            else
            {
                LOG.ErrorFormat( "Unhandled keyboard shortcut - KeyboardShortcut: {0}", keyboardShortcut );
            }
        }

        internal void AddExistingTab( Tab tab )
        {
            LOG.DebugFormat( "AddExistingTab - Tab: {0}", tab );

            TabControllerTag tag = tab.Controller();

            if( this.OwnsLogProcess( tag.Process ) )
            {
                LOG.ErrorFormat( "AddExistingTab - Already own process - PID: {0}", tag.Process.Id );
                return;
            }

            LogTabs.Tabs.Add( tab );
            LogTabs.SelectedTab = tab;
        }

        internal async Task AddNewLogProcess( Process p, String path )
        {
            TabControllerTag tag;
            lock( _tags )
            {
                LOG.DebugFormat( "AddNewLogProcess - Path: {0} - PID: {1}", path, p.Id );
                if( this.OwnsLogProcess( p ) )
                {
                    LOG.DebugFormat( "AddNewLogProcess - Process already under control - Path: {0} - PID: {1}", path, p.Id );
                    return;
                }

                tag = TabControllerTag.CreateController( p, path );
                _tags.Add( tag.Process.Id, tag );
            }

            await tag.WaitForStartup();

            KeyboardShortcutsManager.Instance.AddHandle( tag.Process.MainWindowHandle );

            LogTabs.Tabs.Add( tag.Tab );
            LogTabs.SelectedTab = tag.Tab;

            ShowMe();
        }

        private void RegisterExistingTab( Tab tab )
        {
            LOG.DebugFormat( "RegisterExistingTab - Tab: {0}", tab );

            TabControllerTag tag = tab.Controller();

            if( this.OwnsLogProcess( tag.Process ) )
            {
                LOG.ErrorFormat( "RegisterExistingTab - Already own process - PID: {0}", tag.Process.Id );
                return;
            }

            lock( _tags )
            {
                _tags.Add( tag.Process.Id, tag );
            }

            ShowMe();
        }

        private void RemoveLogProcess( Process p, bool killProcess )
        {
            LOG.DebugFormat( "RemoveLogProcess - PID: {0} - Kill Process: {1}", p.Id, killProcess );

            lock( _tags )
            {
                if( !this.OwnsLogProcess( p ) )
                {
                    LOG.ErrorFormat( "Attempting to remove log not under control - Process ID: {0}", p.Id );
                    return;
                }

                TabControllerTag tag = _tags[ p.Id ];

                _tags.Remove( p.Id );

                tag.Tab.Parent = null;

                if( killProcess )
                {
                    _closedRepos.Push( tag.RepoItem );

                    KeyboardShortcutsManager.Instance.RemoveHandle( p.MainWindowHandle );

                    tag.Close();
                }
            }
        }

        private void RemoveAllLogs()
        {
            lock( _tags )
            {
                LOG.DebugFormat( "RemoveAllLogs - Count: {0}", _tags.Count );

                while( _tags.Count > 0 )
                {
                    RemoveLogProcess( _tags.Values.First().Process, true );
                }
            }
        }

        private void CloseTab( Tab tab )
        {
            TabControllerTag t = tab.Controller();

            LOG.DebugFormat( "Close Tab - Repo: {0} - ID: {1}", t.RepoItem, t.Process.Id );

            RemoveLogProcess( t.Process, true );

            CheckIfLastTab();
        }

        private void CheckIfLastTab()
        {
            if( LogTabs.TabCount == 0
             && Settings.Default.CloseWindowOnLastTabClosed )
            {
                LOG.Debug( "CheckIfLastTab - Closing window after last tab closed" );
                this.Close();
            }
        }

        internal bool OwnsLogProcess( Process logProcess )
        {
            lock( _tags )
            {
                return _tags.ContainsKey( logProcess.Id );
            }
        }

        private void ShowMe()
        {
            this.Show();
            if( this.WindowState == FormWindowState.Minimized )
            {
                this.WindowState = FormWindowState.Normal;
            }
            this.BringToFront();
        }

        private void SaveWindowState()
        {
            if( this.WindowState == FormWindowState.Maximized )
            {
                Settings.Default.Maximized = true;
                Settings.Default.Size = this.RestoreBounds.Size;
            }
            else if( this.WindowState == FormWindowState.Minimized )
            {
                Settings.Default.Maximized = false;
                Settings.Default.Size = this.RestoreBounds.Size;
            }
            else
            {
                Settings.Default.Maximized = false;
                Settings.Default.Size = this.Size;
            }

            Settings.Default.Save();
        }

        private bool ConfirmClose()
        {
            if( !this.Visible )
            {
                return true;
            }
            else if( _tags.Count == 0 )
            {
                return true;
            }
            else if( Settings.Default.ConfirmOnClose )
            {
                TaskDialog confirmationDialog = new TaskDialog()
                {
                    StandardButtons = TaskDialogStandardButtons.Yes | TaskDialogStandardButtons.No,
                    Text = "Are you sure you want to exit?",
                    Caption = "Tabbed Anything",
                    FooterCheckBoxText = "Do not ask me again",
                    FooterCheckBoxChecked = false
                };
                if( confirmationDialog.Show() == TaskDialogResult.Yes )
                {
                    if( confirmationDialog.FooterCheckBoxChecked ?? false )
                    {
                        Settings.Default.ConfirmOnClose = false;
                        Settings.Default.Save();
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return true;
            }
        }

        private void CloseDropDowns( ToolStripItemCollection menuItems )
        {
            foreach( ToolStripDropDownItem dropDownItem in menuItems.OfType<ToolStripDropDownItem>() )
            {
                dropDownItem.HideDropDown();
                CloseDropDowns( dropDownItem.DropDownItems );
            }
        }
    }
}

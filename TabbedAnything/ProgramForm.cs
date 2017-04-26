﻿using Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using TabbedAnything.Properties;
using Tabs;

namespace TabbedAnything
{
    class ProgramForm : Form
    {
        public static readonly int WM_SHOWME = Native.RegisterWindowMessage( "{86ebc75b-fc1b-4370-86bb-2a5daaa51e78}" );

        private static readonly ILog LOG = LogManager.GetLogger( typeof( ProgramForm ) );

        private readonly List<TabbedAnythingForm> _forms = new List<TabbedAnythingForm>();
        private readonly ManagementEventWatcher _watcher;
        private readonly NotifyIcon _notifyIcon;
        private readonly bool _startup;

        private TabbedAnythingForm _activeForm;

        public static ProgramForm Instance { get; private set; }

        public static ProgramForm Create( bool startup )
        {
            if( Instance == null )
            {
                Instance = new ProgramForm( startup );
            }
            return Instance;
        }

        private ProgramForm( bool startup )
        {
            _startup = startup;

            String condition = @"TargetInstance ISA 'Win32_Process' 
                             AND TargetInstance.Name = 'TortoiseGitProc.exe'
                             AND ( TargetInstance.CommandLine LIKE '%/command:log%'
                                OR TargetInstance.CommandLine LIKE '%/command log%'
                                OR TargetInstance.CommandLine LIKE '%-command:log%'
                                OR TargetInstance.CommandLine LIKE '%-command log%' )";
            _watcher = new ManagementEventWatcher( new WqlEventQuery( "__InstanceCreationEvent", new TimeSpan( 10 ), condition ) );
            _watcher.Options.Timeout = new TimeSpan( 0, 1, 0 );
            _watcher.EventArrived += Watcher_EventArrived;
            _watcher.Start();

            _notifyIcon = new NotifyIcon();
            _notifyIcon.Text = "Tabbed Anything";
            _notifyIcon.Visible = true;
            _notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            _notifyIcon.ContextMenuStrip = new ContextMenuStrip();

            ToolStripItem openItem = _notifyIcon.ContextMenuStrip.Items.Add( "Open" );
            openItem.Click += OpenNotifyIconMenuItem_Click;
            ToolStripItem exitItem = _notifyIcon.ContextMenuStrip.Items.Add( "Exit" );
            exitItem.Click += ExitNotifyIconMenuItem_Click;

            this.FormClosing += ProgramForm_FormClosing;

            this.CreateHandle();
        }

        public void CreateNewFromTab( Tab tab, Point location )
        {
            TabbedAnythingForm form = CreateNewTabbedAnything( location );
            form.AddExistingTab( tab );
        }

        protected override void WndProc( ref Message m )
        {
            if( m.Msg == WM_SHOWME )
            {
                CreateNewTabbedAnything( Point.Empty );
            }
            else
            {
                base.WndProc( ref m );
            }
        }

        protected override void SetVisibleCore( bool value )
        {
            base.SetVisibleCore( false );
        }

        protected override void OnHandleCreated( EventArgs e )
        {
            base.OnHandleCreated( e );

            Native.SetParent( this.Handle, Native.HWND_MESSAGE );
            KeyboardShortcutsManager.Create( this.Handle );
            KeyboardShortcutsManager.Instance.KeyboardShortcutPressed += KeyboardShortcutsManager_KeyboardShortcutPressed;

            if( !_startup )
            {
                CreateNewTabbedAnything( Point.Empty );
            }
        }

        protected override void Dispose( bool disposing )
        {
            base.Dispose( disposing );

            if( disposing )
            {
                _notifyIcon.Icon = null;
                _notifyIcon.Visible = false;
                _notifyIcon.Dispose();

                _watcher.Dispose();
            }
        }

        private TabbedAnythingForm CreateNewTabbedAnything( Point createdAtPoint )
        {
            TabbedAnythingForm form = new TabbedAnythingForm( createdAtPoint );
            form.FormClosed += TabbedAnythingForm_FormClosed;
            form.Activated += TabbedAnythingForm_Activated;

            _forms.Add( form );
            _activeForm = form;
            form.Show();

            return form;
        }

        private void ShowLastTabbedAnything()
        {
            if( _activeForm == null )
            {
                CreateNewTabbedAnything( Point.Empty );
            }

            if( _activeForm.WindowState == FormWindowState.Minimized )
            {
                _activeForm.WindowState = FormWindowState.Normal;
            }

            _activeForm.Show();
        }

        private async Task CaptureNewLog( Process p, String repo )
        {
            if( _activeForm == null )
            {
                LOG.Debug( "CaptureNewLog - No active form" );
                CreateNewTabbedAnything( Point.Empty );
            }
            await _activeForm.AddNewLogProcess( p, repo );
        }

        private void KeyboardShortcutsManager_KeyboardShortcutPressed( object sender, KeyboardShortcutPressedEventArgs e )
        {
            LOG.DebugFormat( "KeyboardShortcutPressed - KeyboardShortcut: {0}", e.KeyboardShortcut );
            
            if( _activeForm == null )
            {
                LOG.Error( "KeyboardShortcut received but there is no active form" );
                return;
            }

            _activeForm.HandleKeyboardShortcut( e.KeyboardShortcut );
        }

        private void ProgramForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            LOG.DebugFormat( "ProgramForm_FormClosing - Close Reason: {0}", e.CloseReason );

            _watcher.Stop();

            KeyboardShortcutsManager.Instance.Dispose();
        }

        private void Watcher_EventArrived( object sender, EventArrivedEventArgs e )
        {
            ManagementBaseObject o = (ManagementBaseObject)e.NewEvent[ "TargetInstance" ];
            String commandLine = (String)o[ "CommandLine" ];
            int pid = (int)(UInt32)o[ "ProcessId" ];
            LOG.DebugFormat( "Watcher_EventArrived - CommandLine: {0} - Repo: {1} - PID: {2}", commandLine, path, pid );
            Process p = Process.GetProcessById( pid );

            if( _forms.Any( form => form.OwnsLogProcess( p ) ) )
            {
                LOG.DebugFormat( "Watcher_EventArrived - Log opened by child - PID: {0}", p.Id );
                return;
            }

            this.UiBeginInvoke( (Func<Process, String, Task>)CaptureNewLog, p, path );
        }

        private void NotifyIcon_DoubleClick( object sender, EventArgs e )
        {
            ShowLastTabbedAnything();
        }

        private void OpenNotifyIconMenuItem_Click( object sender, EventArgs e )
        {
            ShowLastTabbedAnything();
        }

        private void ExitNotifyIconMenuItem_Click( object sender, EventArgs e )
        {
            Application.Exit();
        }

        private void TabbedAnythingForm_FormClosed( object sender, FormClosedEventArgs e )
        {
            TabbedAnythingForm form = (TabbedAnythingForm)sender;
            _forms.Remove( form );
            if( form == _activeForm )
            {
                _activeForm = _forms.LastOrDefault();
            }

            if( _forms.Count == 0 )
            {
                if( !Settings.Default.CloseToSystemTray )
                {
                    Application.Exit();
                }
            }
        }

        private void TabbedAnythingForm_Activated( object sender, EventArgs e )
        {
            TabbedAnythingForm form = (TabbedAnythingForm)sender;
            _forms.Remove( form );
            _forms.Add( form );
            _activeForm = form;
        }
    }
}
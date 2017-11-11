using Common;
using log4net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TabbedAnything.Properties;
using Tabs;

namespace TabbedAnything
{
    class TabControllerTag
    {
        private static readonly ILog LOG = LogManager.GetLogger( typeof( TabControllerTag ) );

        public Tab Tab { get; private set; }
        public Process Process { get; private set; }

        private readonly Native.WinEventDelegate _winProcDelegate;
        private readonly IntPtr _hook;

        public static TabControllerTag CreateController( Process p )
        {
            Tab t = new Tab( Native.GetWindowText( p.MainWindowHandle ) );

            TabControllerTag tag = new TabControllerTag( t, p );

            t.Tag = tag;

            return tag;
        }

        private TabControllerTag( Tab tab, Process process )
        {
            Tab = tab;
            Process = process;

            Tab.Resize += Tab_Resize;

            Process.Exited += Process_Exited;
            Process.EnableRaisingEvents = true;

            _winProcDelegate = new Native.WinEventDelegate( WinEventProc );
            _hook = Native.SetWinEventHook( Native.EVENT_OBJECT_NAMECHANGE, Native.EVENT_OBJECT_NAMECHANGE, IntPtr.Zero, _winProcDelegate, (uint)process.Id, 0, Native.WINEVENT_OUTOFCONTEXT );
        }

        public async Task WaitForStartup()
        {
            LOG.DebugFormat( "WaitForStartup - Start Wait for MainWindowHandle - PID: {0}", this.Process.Id );
            while( !this.Process.HasExited && this.Process.MainWindowHandle == IntPtr.Zero )
            {
                await Task.Delay( 10 );
            }
            LOG.DebugFormat( "WaitForStartup - End Wait for MainWindowHandle - PID: {0}", this.Process.Id );

            Native.RemoveBorder( this.Process.MainWindowHandle );
            Native.SetWindowParent( this.Process.MainWindowHandle, this.Tab );
            this.ResizeTab();
        }

        private void EndProcess()
        {
            if( !this.Process.CloseMainWindow() )
            {
                this.Process.Kill();
            }
        }

        public void Close()
        {
            this.Tab.Resize -= Tab_Resize;
            this.Tab.Parent = null;

            this.Process.EnableRaisingEvents = false;
            this.Process.Exited -= Process_Exited;

            Native.UnhookWinEvent( _hook );

            if( !this.Process.HasExited )
            {
                Task.Run( (Action)this.EndProcess );
            }
        }

        private void ResizeTab()
        {
            Size sizeDiff = Native.ResizeToParent( this.Process.MainWindowHandle, this.Tab );

            Form form = this.Tab.FindForm();

            if( form != null )
            {
                if( sizeDiff.Width > 0 )
                {
                    form.Width += sizeDiff.Width;
                    form.MinimumSize = new Size( form.Width, form.MinimumSize.Height );
                }

                if( sizeDiff.Height > 0 )
                {
                    form.Height += sizeDiff.Height;
                    form.MinimumSize = new Size( form.MinimumSize.Width, form.Height );
                }
            }
        }

        private void Tab_Resize( object sender, EventArgs e )
        {
            if( this.Tab.FindForm()?.WindowState != FormWindowState.Minimized )
            {
                ResizeTab();
            }
        }

        private void Process_Exited( object sender, EventArgs e )
        {
            this.Tab.UiBeginInvoke( (Action)( () => this.Tab.Parent = null ) );
        }

        private void WinEventProc( IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime )
        {
            if( hwnd == this.Process.MainWindowHandle )
            {
                this.Tab.Text = Native.GetWindowText( this.Process.MainWindowHandle );
            }
        }
    }

    static class TabExtensions
    {
        public static TabControllerTag Controller( this Tab tab )
        {
            return (TabControllerTag)tab.Tag;
        }
    }
}

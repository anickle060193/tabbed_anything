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

        public static TabControllerTag CreateController( Process p )
        {
            Tab t = new Tab( "Tab" );

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
    }

    static class TabExtensions
    {
        public static TabControllerTag Controller( this Tab tab )
        {
            return (TabControllerTag)tab.Tag;
        }
    }
}

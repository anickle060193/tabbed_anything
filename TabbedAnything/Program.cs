using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tabs;

namespace TabbedAnything
{
    static class Program
    {
        private static ILog LOG = LogManager.GetLogger( typeof( Program ) );

        private static readonly Mutex _mutex = new Mutex( true, "{3a39a5c1-fac2-4059-81d4-5018abfa5142}" );

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main( String[] args )
        {
            LOG.DebugFormat( "Application Startup - Args: {0}", String.Join( ", ", args ) );
            LOG.DebugFormat( "Version: {0}", Assembly.GetExecutingAssembly().GetName().Version );

            Application.ThreadException += Application_ThreadException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

            if( !TabbedAnythingUtil.VerifyAssemblyVersions() )
            {
                return;
            }

            Arguments a = new Arguments();
            if( CommandLine.Parser.Default.ParseArguments( args, a ) )
            {
                if( _mutex.WaitOne( TimeSpan.Zero, true ) )
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault( false );

                    LOG.Debug( "Starting Tabbed Anything" );
                    Application.Run( ProgramForm.Create( a.Startup ) );
                    _mutex.ReleaseMutex();
                }
                else
                {
                    LOG.Debug( "Tabbed Anything instance already exists" );
                    Native.PostMessage( (IntPtr)Native.HWND_BROADCAST, ProgramForm.WM_SHOWME, IntPtr.Zero, IntPtr.Zero );
                }
            }

            LOG.Debug( "Application End" );
        }

        private static void CurrentDomain_UnhandledException( object sender, UnhandledExceptionEventArgs e )
        {
            LOG.Fatal( "AppDomain UnhandledException Occurred", (Exception)e.ExceptionObject );
        }

        private static void Application_ThreadException( object sender, ThreadExceptionEventArgs e )
        {
            LOG.Fatal( "Application ThreadException Occurred", e.Exception );
        }
    }
}

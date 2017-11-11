using Common;
using log4net;
using log4net.Appender;
using log4net.Repository.Hierarchy;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using TabbedAnything.Properties;
using Tabs;

namespace TabbedAnything
{
    partial class TabbedAnythingForm
    {
        private void InitializeEventHandlers()
        {
            this.VisibleChanged += TabbedAnythingForm_VisibleChanged;
            this.ResizeEnd += TabbedAnythingForm_ResizeEnd;
            this.FormClosing += TabbedAnythingForm_FormClosing;

            ProcessTabs.NewTabClick += ProcessTabs_NewTabClick;
            ProcessTabs.TabAdded += ProcessTabs_TabAdded;
            ProcessTabs.TabRemoved += ProcessTabs_TabRemoved;
            ProcessTabs.TabClosed += ProcessTabs_TabClosed;
            ProcessTabs.TabPulledOut += ProcessTabs_TabPulledOut;
            ProcessTabs.SelectedTabChanged += ProcessTabs_SelectedTabChanged;

            OpenNewTabMenuItem.Click += OpenNewTabMenuItem_Click;
            SettingsMenuItem.Click += SettingsMenuItem_Click;
            ShowDebugLogMenuItem.Click += ShowDebugLogMenuItem_Click;
            AboutMenuItem.Click += AboutMenuItem_Click;
            ExitMenuItem.Click += ExitMenuItem_Click;

            CloseTabMenuItem.Click += CloseTabMenuItem_Click;
        }

        private async void TabbedAnythingForm_VisibleChanged( object sender, EventArgs e )
        {
            LOG.DebugFormat( "Visible Changed - Visible: {0}", this.Visible );
            if( this.Visible )
            {
                DateTime lastUpdatePromptTime = Settings.Default.LastUpdatePromptTime;
                DateTime now = DateTime.Now;
                TimeSpan difference = now - lastUpdatePromptTime;
                var inject = new {
                    lastUpdatePromptTime = lastUpdatePromptTime,
                    now = now,
                    difference = difference
                };
                LOG.DebugInject( "Visible Changed - Last Update Prompt Time: {lastUpdatePromptTime} - Now: {now} - Difference: {difference}", inject );
                if( difference >= TimeSpan.FromDays( 1 ) )
                {
                    Version newestVersion = await TabbedAnythingUtil.IsUpToDate();
                    if( newestVersion != null )
                    {
                        LOG.DebugFormat( "Newest Version: {0}", newestVersion );

                        if( DialogResult.Yes == MessageBox.Show( "A new version of Tabbed Anything is available. Update now?\n\nNote: This will exit Tabbed Anything.", "New Update", MessageBoxButtons.YesNo ) )
                        {
                            LOG.Debug( "Update Prompt: Yes" );
                            Settings.Default.LastUpdatePromptTime = DateTime.MinValue;
                            Settings.Default.Save();

                            if( await TabbedAnythingUtil.UpdateApplication( newestVersion ) )
                            {
                                LOG.Debug( "Update - Exiting Application" );
                                Application.Exit();
                            }
                            else
                            {
                                LOG.Debug( "Update - Error occurred" );
                                MessageBox.Show( "There was an error updating Tabbed Anything. Try again later." );
                            }
                        }
                        else
                        {
                            LOG.Debug( "Update Prompt: No" );
                            Settings.Default.LastUpdatePromptTime = now;
                            Settings.Default.Save();

                            MessageBox.Show( "To update Tabbed Anything at a later date, go to Options->About and click Update.", "New Update", MessageBoxButtons.OK );
                        }
                    }
                }
            }
        }

        private void TabbedAnythingForm_ResizeEnd( object sender, EventArgs e )
        {
            SaveWindowState();
        }

        private void TabbedAnythingForm_FormClosing( object sender, FormClosingEventArgs e )
        {
            LOG.DebugFormat( "FormClosing - Reason: {0}", e.CloseReason );

            if( e.CloseReason == CloseReason.UserClosing )
            {
                if( !this.ConfirmClose() )
                {
                    LOG.Debug( "FormClosing - Cancelled on confirm close" );
                    e.Cancel = true;
                    return;
                }
            }

            ProcessTabs.TabRemoved -= ProcessTabs_TabRemoved;
            ProcessTabs.TabClosed -= ProcessTabs_TabClosed;

            RemoveAllProcesses();
            SaveWindowState();
        }

        private async void ProcessTabs_NewTabClick( object sender, EventArgs e )
        {
            await StartNewProcess();
        }

        private void ProcessTabs_TabAdded( object sender, TabAddedEventArgs e )
        {
            LOG.DebugFormat( "TabAdded - Tab: {0}", e.Tab );

            RegisterExistingTab( e.Tab );
        }

        private void ProcessTabs_TabRemoved( object sender, TabRemovedEventArgs e )
        {
            LOG.DebugFormat( "TabRemoved - Tab: {0}", e.Tab );

            this.RemoveProcess( e.Tab.Controller().Process, false );

            CheckIfLastTab();
        }

        private void ProcessTabs_TabClosed( object sender, TabClosedEventArgs e )
        {
            LOG.Debug( "Tab Closed" );

            CloseTab( e.Tab );
        }

        private void ProcessTabs_TabPulledOut( object sender, TabPulledOutEventArgs e )
        {
            ProgramForm.Instance.CreateNewFromTab( e.Tab, e.Location );
        }

        private void ProcessTabs_SelectedTabChanged( object sender, EventArgs e )
        {
            if( ProcessTabs.SelectedTab != null )
            {
                this.Text = ProcessTabs.SelectedTab.Text + " - Tabbed Anything";
            }
            else
            {
                this.Text = "Tabbed Anything";
            }
        }

        private async void OpenNewTabMenuItem_Click( object sender, EventArgs e )
        {
            await StartNewProcess();
        }

        private void SettingsMenuItem_Click( object sender, EventArgs e )
        {
            if( SettingsForm.ShowSettingsDialog() )
            {
                UpdateFromSettings( false );
            }
        }

        private void ShowDebugLogMenuItem_Click( object sender, EventArgs e )
        {
            FileAppender rootAppender = ( (Hierarchy)LogManager.GetRepository() ).Root.Appenders.OfType<FileAppender>().FirstOrDefault();
            if( rootAppender != null )
            {
                Util.OpenInExplorer( rootAppender.File );
            }
        }

        private void AboutMenuItem_Click( object sender, EventArgs e )
        {
            AboutBox.ShowAbout();
        }

        private void ExitMenuItem_Click( object sender, EventArgs e )
        {
            this.Close();
        }

        private void CloseTabMenuItem_Click( object sender, EventArgs e )
        {
            LOG.Debug( "Tab Menu Item - Close Tab Click" );
            CloseTab( ProcessTabs.SelectedTab );
        }
    }
}

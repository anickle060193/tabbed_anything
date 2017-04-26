using Common;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace TabbedAnything.Properties
{
    internal sealed partial class Settings
    {
        private static readonly ILog LOG = LogManager.GetLogger( typeof( Settings ) );

        public Dictionary<KeyboardShortcuts, Shortcut> KeyboardShortcuts
        {
            get
            {
                Dictionary<KeyboardShortcuts, Shortcut> keyboardShortcuts = null;
                try
                {
                    String keyboardShortcutsString = Settings.Default.KeyboardShortcutsString;
                    keyboardShortcuts = JsonConvert.DeserializeObject<Dictionary<KeyboardShortcuts, Shortcut>>( keyboardShortcutsString );
                }
                catch( JsonException e )
                {
                    LOG.ErrorFormat( "Failed to deserialize KeyboardShortcuts setting - KeyboardShortcuts: {0}", Settings.Default.KeyboardShortcutsString );
                    LOG.Error( e );
                }
                return keyboardShortcuts ?? new Dictionary<KeyboardShortcuts, Shortcut>();
            }

            set
            {
                try
                {
                    Settings.Default.KeyboardShortcutsString = JsonConvert.SerializeObject( value, Formatting.Indented );
                }
                catch( JsonException e )
                {
                    LOG.Error( "Failed to serialize KeyboardShortcuts.", e );
                }
            }
        }


        public Settings()
        {
            this.SettingsLoaded += Settings_SettingsLoaded;
            this.SettingChanging += Settings_SettingChanging;
        }

        private void Settings_SettingChanging( object sender, SettingChangingEventArgs e )
        {
        }

        private void Settings_SettingsLoaded( object sender, SettingsLoadedEventArgs e )
        {
            if( Settings.Default.UpgradeRequired )
            {
                LOG.Debug( "Upgrading settings" );
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
            }

            if( Settings.Default.KeyboardShortcutsString == null )
            {
                Settings.Default.KeyboardShortcutsString = "";
            }

            Settings.Default.Save();
        }
    }
}

using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TabbedAnything.Properties;

namespace TabbedAnything
{
    public partial class SettingsForm : Form
    {
        public static bool ShowSettingsDialog()
        {
            SettingsForm f = new SettingsForm();

            f.KeyboardShortcuts = Settings.Default.KeyboardShortcuts;

            f.ConfirmOnClose = Settings.Default.ConfirmOnClose;
            f.CloseWindowOnLastTabClosed = Settings.Default.CloseWindowOnLastTabClosed;
            f.CloseToSystemTray = Settings.Default.CloseToSystemTray;

            if( f.ShowDialog() == DialogResult.OK )
            {
                Settings.Default.KeyboardShortcuts = f.KeyboardShortcuts;

                Settings.Default.ConfirmOnClose = f.ConfirmOnClose;
                Settings.Default.CloseWindowOnLastTabClosed = f.CloseWindowOnLastTabClosed;
                Settings.Default.CloseToSystemTray = f.CloseToSystemTray;

                Settings.Default.Save();

                return true;
            }
            else
            {
                return false;
            }
        }

        public Dictionary<KeyboardShortcuts, Shortcut> KeyboardShortcuts
        {
            get
            {
                return _shortcutTextboxes.ToDictionary( pair => pair.Key, pair => (Shortcut)pair.Value.Tag );
            }

            set
            {
                foreach( KeyboardShortcuts keyboardShortcut in Enum.GetValues( typeof( KeyboardShortcuts ) ) )
                {
                    Shortcut shortcut;
                    if( value.TryGetValue( keyboardShortcut, out shortcut ) )
                    {
                        UpdateShortcutTextBox( _shortcutTextboxes[ keyboardShortcut ], shortcut );
                    }
                }
            }
        }

        public bool ConfirmOnClose
        {
            get
            {
                return ConfirmOnCloseCheck.Checked;
            }

            set
            {
                ConfirmOnCloseCheck.Checked = value;
            }
        }

        public bool CloseWindowOnLastTabClosed
        {
            get
            {
                return CloseWindowOnLastTabClosedCheck.Checked;
            }

            set
            {
                CloseWindowOnLastTabClosedCheck.Checked = value;
            }
        }

        public bool CloseToSystemTray
        {
            get
            {
                return CloseToSystemTrayCheck.Checked;
            }

            set
            {
                CloseToSystemTrayCheck.Checked = value; 
            }
        }

        private readonly Dictionary<KeyboardShortcuts, TextBox> _shortcutTextboxes = new Dictionary<KeyboardShortcuts, TextBox>();

        public SettingsForm()
        {
            InitializeComponent();

            KeyBoardShortcutsTableLayout.RowStyles.Clear();

            foreach( KeyboardShortcuts keyboardShortcut in Enum.GetValues( typeof( KeyboardShortcuts ) ) )
            {
                Label shortcutLabel = new Label()
                {
                    Anchor = AnchorStyles.Left,
                    AutoSize = true,
                    Text = keyboardShortcut.GetDescription()
                };

                TextBox shortcutText = new TextBox()
                {
                    AcceptsTab = true,
                    Anchor = AnchorStyles.Left,
                    Width = 180,
                    ReadOnly = true,
                    ShortcutsEnabled = false,
                    TabStop = false
                };

                shortcutText.Enter += ShortcutText_Enter;
                shortcutText.PreviewKeyDown += ShortcutTextBox_PreviewKeyDown;
                shortcutText.KeyDown += ShortcutText_KeyDown;
                _shortcutTextboxes.Add( keyboardShortcut, shortcutText );

                KeyBoardShortcutsTableLayout.RowStyles.Add( new RowStyle( SizeType.AutoSize ) );

                KeyBoardShortcutsTableLayout.Controls.Add( shortcutLabel );
                KeyBoardShortcutsTableLayout.Controls.Add( shortcutText );
            }

            KeyBoardShortcutsTableLayout.RowStyles.Add( new RowStyle( SizeType.Percent, 1.0f ) );
            KeyBoardShortcutsTableLayout.RowCount++;
        }

        protected override bool ProcessTabKey( bool forward )
        {
            if( _shortcutTextboxes.Values.Contains( this.ActiveControl ) )
            {
                return true;
            }
            else
            {
                return base.ProcessTabKey( forward );
            }
        }

        private void ShortcutText_Enter( object sender, EventArgs e )
        {
            TextBox shortcutText = (TextBox)sender;

            UpdateShortcutTextBox( shortcutText, Shortcut.Empty );
        }

        private void ShortcutTextBox_PreviewKeyDown( object sender, PreviewKeyDownEventArgs e )
        {
            TextBox shortcutText = (TextBox)sender;

            Shortcut shortcut = Shortcut.FromKeyEventArgs( e );
            UpdateShortcutTextBox( shortcutText, shortcut );
        }

        private void ShortcutText_KeyDown( object sender, KeyEventArgs e )
        {
            e.SuppressKeyPress = true;
            e.Handled = true;
        }

        private void UpdateShortcutTextBox( TextBox textbox, Shortcut shortcut )
        {
            textbox.Tag = shortcut;
            textbox.Text = shortcut?.Text;
            textbox.SelectionStart = textbox.TextLength;
        }
    }
}

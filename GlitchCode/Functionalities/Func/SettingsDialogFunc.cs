using GlitchCode.Managers;
using GlitchCode.Pages;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms;
using System.Windows.Media;
using Brush = System.Windows.Media.Brush;
using Button = System.Windows.Controls.Button;
using ComboBox = System.Windows.Controls.ComboBox;
using Control = System.Windows.Controls.Control;
using TextBox = System.Windows.Controls.TextBox;

namespace GlitchCode.Functionalities.Func
{
    class SettingsDialogFunc
    {
        static BrushConverter converter = new BrushConverter();
        private SettingsPage settingsPage;

        public SettingsDialogFunc(SettingsPage settingsPage)
        {
            this.settingsPage = settingsPage;
            loadSettings(new Border[] { settingsPage.Border1 });
            settingsPage.AccentColorOptionControl.Text = string.IsNullOrEmpty(Properties.Settings.Default.AccentColor) ? "#------" : Properties.Settings.Default.AccentColor;
            if (settingsPage.AccentColorOptionControl.Text != "#------") settingsPage.AccentColorOptionControl.Background = (Brush)converter.ConvertFromString(settingsPage.AccentColorOptionControl.Text);
            if (SettingsManager.tabItems.Count > 0)
            {
                settingsPage.AddonDivider.Visibility = Visibility.Visible;
                foreach (var tabItem in SettingsManager.tabItems)
                {
                    settingsPage.TabControl.Items.Add(tabItem);
                }
            }
            foreach (Button button in ((Grid)settingsPage.Content).Children.OfType<Button>())
            {
                button.Click += button_Click;
            }
            settingsPage.ColorPickerButton.Click += colorPickerButton_Click;
        }

        private void colorPickerButton_Click(object sender, RoutedEventArgs e)
        {
            ColorDialog colorDialog = new ColorDialog();
            if (settingsPage.AccentColorOptionControl.Text != "#------") colorDialog.Color = ColorTranslator.FromHtml(settingsPage.AccentColorOptionControl.Text);
            colorDialog.FullOpen = true;
            colorDialog.AnyColor = false;
            if (colorDialog.ShowDialog() == DialogResult.OK)
            {
                settingsPage.AccentColorOptionControl.Text = "#" + (colorDialog.Color.ToArgb() & 0x00FFFFFF).ToString("X6");
                settingsPage.AccentColorOptionControl.Background = (Brush)converter.ConvertFromString(settingsPage.AccentColorOptionControl.Text);
            }
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            switch (button.Name)
            {
                case "SaveButton":
                    SettingsFunc.saveSettings(new Grid[] { (Grid)settingsPage.GeneralTabItem.Content, (Grid)settingsPage.PersonalizationTabItem.Content });
                    SettingsFunc.refreshSettings();
                    break;
                case "ResetButton":
                    Properties.Settings.Default.Reset();
                    SettingsFunc.refreshSettings();
                    break;
            }
            (settingsPage.Parent as Window).Close();
        }

        private void loadSettings(Border[] borders)
        {
            foreach (Border border in borders)
            {
                Grid grid = border.Child as Grid;
                foreach (DockPanel dockPanel in grid.Children)
                {
                    Control control = dockPanel.Children.OfType<Control>().Single(item => item.Name == dockPanel.Name + "OptionControl");
                    if (control.GetType() == typeof(ToggleButton)) ((ToggleButton)control).IsChecked = (bool)Properties.Settings.Default[control.Tag.ToString()];
                    if (control.GetType() == typeof(ComboBox)) ((ComboBox)control).Text = (string)Properties.Settings.Default[control.Tag.ToString()];
                    if (control.GetType() == typeof(TextBox)) ((TextBox)control).Text = (string)Properties.Settings.Default[control.Tag.ToString()];
                }
            }
        }
    }
}

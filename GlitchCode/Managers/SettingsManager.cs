using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace GlitchCode.Managers
{
    public static class SettingsManager
    {
        public static void saveSetting(Control control)
        {
            Properties.Settings.Default[control.Tag.ToString()] = getValue(control);
        }

        static object getValue(Control control)
        {
            if (control.GetType() == typeof(ToggleButton)) return ((ToggleButton)control).IsChecked;
            if (control.GetType() == typeof(ComboBox)) return ((ComboBoxItem)((ComboBox)control).SelectedItem).Content;
            if (control.GetType() == typeof(TextBox) && ((TextBox)control).Text != "#------") return ((TextBox)control).Text;
            return null;
        }

        public static List<TabItem> tabItems = new List<TabItem>();

        public static void createAddonSettingsPage(string name, UserControl settingPage)
        {
            tabItems.Add(new TabItem() { Header = name, Content = settingPage });
        }
    }
}

using GlitchCode.Managers;
using HandyControl.Themes;
using System;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Media;

namespace GlitchCode.Functionalities.Func
{
    class SettingsFunc : IFuncionality
    {
        static BrushConverter converter = new BrushConverter();

        public void onEnable()
        {
            refreshSettings();
        }

        public static void refreshSettings()
        {
            Properties.Settings.Default.Save();
            loadTheme();
        }

        static void loadTheme()
        {
            if (Properties.Settings.Default.Theme == "Sync")
                ThemeManager.Current.UsingSystemTheme = true;
            else
                ThemeManager.Current.ApplicationTheme = (ApplicationTheme?)Enum.Parse(typeof(ApplicationTheme), Properties.Settings.Default.Theme, true);
            try
            {
                ThemeManager.Current.AccentColor = (Brush)converter.ConvertFromString(Properties.Settings.Default.AccentColor);
            }
            catch (Exception)
            {
                ThemeManager.Current.AccentColor = null;
            }
        }

        public static void saveSettings(Grid[] grids)
        {
            foreach (Grid grid in grids)
            {
                foreach (Border border in grid.Children)
                {
                    Grid borderGrid = border.Child as Grid;
                    foreach (DockPanel dockPanel in borderGrid.Children)
                    {
                        SettingsManager.saveSetting(dockPanel.Children.OfType<Control>().Single(item => item.Name == dockPanel.Name + "OptionControl"));
                    }
                }
            }
        }
    }
}

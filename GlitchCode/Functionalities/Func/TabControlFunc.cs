using GlitchCode.Managers;
using System.Linq;
using System.Windows.Controls;
using TabControl = HandyControl.Controls.TabControl;
using TabItem = HandyControl.Controls.TabItem;

namespace GlitchCode.Functionalities.Func
{
    internal class TabControlFunc : IFuncionality
    {
        TabControl tabControl = (((MainWindow.Items.Find(item => item.Name == "MainContent") as Border)
            .Child as Grid)
            .Children
            .OfType<Border>()
            .Single(item => item.Name == "Editor")
            .Child as Grid)
            .Children
            .OfType<TabControl>()
            .Single(item => item.Name == "TabControl");

        public void Load()
        {
            tabControl.SelectionChanged += tabControl_SelectionChanged;
            DiscordRPCManager.UpdateFile((tabControl.SelectedItem as TabItem).Header.ToString());
        }

        private void tabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (tabControl.SelectedItem != null)
                DiscordRPCManager.UpdateFile((tabControl.SelectedItem as TabItem).Header.ToString());
            else
                DiscordRPCManager.UpdateFile("No File");
        }
    }
}

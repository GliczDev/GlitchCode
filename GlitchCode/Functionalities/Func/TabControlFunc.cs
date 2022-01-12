using GlitchCode.Managers;
using System.Windows.Controls;
using TabControl = HandyControl.Controls.TabControl;
using TabItem = HandyControl.Controls.TabItem;

namespace GlitchCode.Functionalities.Func
{
    class TabControlFunc : IFuncionality
    {
        TabControl tabControl = WindowManager.getMainWindow().TabControl;

        public void onEnable()
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

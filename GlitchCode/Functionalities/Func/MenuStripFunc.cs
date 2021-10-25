using GlitchCode.Managers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GlitchCode.Functionalities.Func
{
    class MenuStripFunc : IFuncionality
    {
        Menu menuStrip = MainWindow.TitleBarItems.Find(item => item.Name == "MenuStrip") as Menu;

        public void Load()
        {
            foreach (MenuItem menuItem in (menuStrip.Items[0] as MenuItem).Items.OfType<MenuItem>())
            {
                menuItem.Click += fileMenuItems_Click;
            }
        }

        void fileMenuItems_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            switch (menuItem.Name)
            {
                case "newFile_MenuItem":
                    FileManager.createNewFile(true);
                    break;
                case "open_MenuItem":
                    FileManager.openFile();
                    break;
                case "save_MenuItem":
                    FileManager.saveFile();
                    break;
                case "saveAs_MenuItem":
                    FileManager.saveAsFile();
                    break;
                case "close_MenuItem":
                    Environment.Exit(0);
                    break;
            }
        }
    }
}

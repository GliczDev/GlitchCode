using GlitchCode.Managers;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GlitchCode.Functionalities.Func
{
    class MenuStripFunc : IFuncionality
    {
        Menu menuStrip = WindowManager.getMainWindow().MenuStrip;

        public void onEnable()
        {
            foreach (MenuItem menuItem in (menuStrip.Items[0] as MenuItem).Items.OfType<MenuItem>())
            {
                menuItem.Click += fileMenuItems_Click;
            }
            foreach (MenuItem menuItem in (menuStrip.Items[1] as MenuItem).Items.OfType<MenuItem>())
            {
                menuItem.Click += editMenuItems_Click;
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

        void editMenuItems_Click(object sender, RoutedEventArgs e)
        {
            MenuItem menuItem = (MenuItem)sender;
            switch (menuItem.Name)
            {
                case "undo_MenuItem":
                    FileManager.getTextEditor().Undo();
                    break;
                case "redo_MenuItem":
                    FileManager.getTextEditor().Redo();
                    break;
                case "cut_MenuItem":
                    FileManager.getTextEditor().Cut();
                    break;
                case "copy_MenuItem":
                    FileManager.getTextEditor().Copy();
                    break;
                case "paste_MenuItem":
                    FileManager.getTextEditor().Paste();
                    break;
                case "selectAll_MenuItem":
                    FileManager.getTextEditor().SelectAll();
                    break;
            }
        }
    }
}

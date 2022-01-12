using GlitchCode.Managers;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace GlitchCode.Functionalities.Func
{
    class ContextMenuFunc : IFuncionality
    {
        ContextMenu contextMenu = (ContextMenu)App.Current.TryFindResource("ContextMenu");

        public void onEnable()
        {
            foreach (MenuItem menuitem in contextMenu.Items.OfType<MenuItem>())
            {
                menuitem.Click += contextMenuItems_Click;
            }
        }

        void contextMenuItems_Click(object sender, RoutedEventArgs e)
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

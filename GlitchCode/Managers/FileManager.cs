using GlitchCode.Pages;
using HandyControl.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using Application = System.Windows.Application;
using TabControl = HandyControl.Controls.TabControl;
using TabItem = HandyControl.Controls.TabItem;

namespace GlitchCode.Managers
{
    public static class FileManager
    {
        static TabControl tabControl = (((MainWindow.Items.Find(item => item.Name == "MainContent") as Border)
            .Child as Grid)
            .Children
            .OfType<Border>()
            .Single(item => item.Name == "Editor")
            .Child as Grid)
            .Children
            .OfType<TabControl>()
            .First();

        static Regex regex = new Regex("Untitled-[0-9]+");

        static int untitledCount()
        {
            int itemIndex = 1;
            List<TabItem> tabItems = new List<TabItem>();
            List<int> tabItemsIndexes = new List<int>() { 1 };
            foreach (TabItem tabItem in tabControl.Items.OfType<TabItem>()
                .Where(item => regex.Matches(item.Header.ToString())
                .Count >= 1))
            {
                tabItems.Add(tabItem);
                tabItemsIndexes.Add(int.Parse(tabItem.Header.ToString().Replace("Untitled-", "")) + 1);
            }
            for (int i = 1; i <= tabItemsIndexes.Max(); i++)
            {
                if (tabItems.Find(item => item.Header.ToString().EndsWith(i.ToString())) == null)
                {
                    itemIndex = i;
                    break;
                }
            }
            return itemIndex;
        }

        static int afterCloseIndex()
        {
            int i = tabControl.SelectedIndex - 1;
            if (i == -2)
                i += 1;
            return i;
        }

        static void tabItem_Closing(object sender, EventArgs e)
        {
            Growl.Info("Successfully closed file");
        }

        static ICSharpCode.AvalonEdit.TextEditor getTextEditor()
        {
            ICSharpCode.AvalonEdit.TextEditor textEditor = null;
            TabItem tp = tabControl.SelectedItem as TabItem;
            if (tp != null)
                textEditor = tp.Content as ICSharpCode.AvalonEdit.TextEditor;
            return textEditor;
        }

        public static void createNewFile(bool growl)
        {
            TabItem tabItem = new TabItem
            {
                Header = "Untitled-" + untitledCount(),
                Content = new ICSharpCode.AvalonEdit.TextEditor()
                {
                    Style = Application.Current.FindResource("TextEditor") as Style
                },
                IsSelected = true
            };
            tabItem.Closing += tabItem_Closing;
            tabControl.Items.Add(tabItem);
            if (growl)
                Growl.Info("Successfully created new file");
        }

        public static void createWelcomeTabItem()
        {
            TabItem tabItem = new TabItem
            {
                Header = "Welcome",
                Content = new WelcomePage()
                {
                    FontStyle = FontStyles.Normal
                },
                FontStyle = FontStyles.Italic,
                IsSelected = true
            };
            tabItem.SetResourceReference(TabItem.HeaderProperty, "TabItemWelcome");
            tabItem.Closing += tabItem_Closing;
            tabControl.Items.Add(tabItem);
        }

        public static void closeFile(TabItem tabItem = null)
        {
            tabControl.SelectedIndex = afterCloseIndex();
            if (tabItem == null)
            {
                tabControl.Items.Remove(tabControl.SelectedItem);
                return;
            }
            tabControl.Items.Remove(tabItem);
            Growl.Info("Successfully closed file");
        }

        public static void openFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            if (openDialog.ShowDialog() != DialogResult.OK)
                return;
            createNewFile(false);
            getTextEditor().Load(openDialog.FileName);
            TabItem currentTabItem = tabControl.SelectedItem as TabItem;
            currentTabItem.ToolTip = openDialog.FileName;
            currentTabItem.Header = openDialog.SafeFileName;
            Growl.Info("Successfully opened file");
        }

        public static void saveFile()
        {
            if (getTextEditor() == null)
                return;
            TabItem currentTabItem = tabControl.SelectedItem as TabItem;
            if (currentTabItem.ToolTip != null && currentTabItem.ToolTip.ToString() != "")
            {
                getTextEditor().Save(currentTabItem.ToolTip.ToString());
                Growl.Info("Successfully saved file");
                return;
            }
            saveAsFile();
        }

        public static void saveAsFile()
        {
            if (getTextEditor() == null)
                return;
            SaveFileDialog saveDialog = new SaveFileDialog();
            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;
            getTextEditor().Save(saveDialog.FileName);
            TabItem currentTabItem = tabControl.SelectedItem as TabItem;
            currentTabItem.ToolTip = saveDialog.FileName;
            currentTabItem.Header = new FileInfo(saveDialog.FileName).Name;
            Growl.Info("Successfully saved file");
        }
    }
}

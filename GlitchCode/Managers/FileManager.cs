using GlitchCode.Pages;
using HandyControl.Controls;
using ICSharpCode.AvalonEdit;
using ICSharpCode.AvalonEdit.Search;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using Application = System.Windows.Application;
using TabControl = HandyControl.Controls.TabControl;
using TabItem = HandyControl.Controls.TabItem;

namespace GlitchCode.Managers
{
    public static class FileManager
    {
        static TabControl tabControl = WindowManager.getMainWindow().TabControl;

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
            Growl.Success("Successfully closed file");
        }

        public static TextEditor getTextEditor()
        {
            TextEditor textEditor = null;
            TabItem tp = tabControl.SelectedItem as TabItem;
            if (tp != null)
                textEditor = tp.Content as TextEditor;
            return textEditor;
        }

        public static void createNewFile(bool growl, Geometry geometry = null, object content = null)
        {
            TabItem tabItem = new TabItem
            {
                Header = "Untitled-" + untitledCount(),
                IsSelected = true
            };
            if (content == null)
            {
                TextEditor textEditor = new TextEditor()
                {
                    Style = Application.Current.FindResource("TextEditor") as Style
                };
                textEditor.Options.EnableHyperlinks = false;
                textEditor.Options.HighlightCurrentLine = true;
                textEditor.TextArea.TextView.CurrentLineBackground = Brushes.Transparent;
                textEditor.TextArea.TextView.CurrentLineBorder = new Pen((Brush)App.Current.FindResource("ThirdlyRegionBrush"), 2);
                SearchPanel searchPanel = SearchPanel.Install(textEditor);
                searchPanel.Margin = new Thickness(5, 5, 5, 5);
                searchPanel.Style = (Style)Application.Current.FindResource("SearchPanel");
                tabItem.Content = textEditor;
                tabItem.Closing += tabItem_Closing;
            }
            IconElement.SetGeometry(tabItem,
                (geometry == null)
                ? Geometry.Parse("M224 128L224 0H48C21.49 0 0 21.49 0 48v416C0 490.5 21.49 512 48 512h288c26.51 0 48-21.49 48-48V160h-127.1C238.3 160 224 145.7 224 128zM154.1 353.8c7.812 7.812 7.812 20.5 0 28.31C150.2 386.1 145.1 388 140 388s-10.23-1.938-14.14-5.844l-48-48c-7.812-7.812-7.812-20.5 0-28.31l48-48c7.812-7.812 20.47-7.812 28.28 0s7.812 20.5 0 28.31L120.3 320L154.1 353.8zM306.1 305.8c7.812 7.812 7.812 20.5 0 28.31l-48 48C254.2 386.1 249.1 388 244 388s-10.23-1.938-14.14-5.844c-7.812-7.812-7.812-20.5 0-28.31L263.7 320l-33.86-33.84c-7.812-7.812-7.812-20.5 0-28.31s20.47-7.812 28.28 0L306.1 305.8zM256 0v128h128L256 0z")
                : geometry);
            IconElement.SetWidth(tabItem, 15);
            IconElement.SetHeight(tabItem, 15);
            tabControl.Items.Add(tabItem);
            if (growl)
                Growl.Success("Successfully created new file");
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
            IconElement.SetGeometry(tabItem, Geometry.Parse("M448 64H64C28.63 64 0 92.63 0 128v256c0 35.38 28.62 64 64 64h384c35.38 0 64-28.62 64-64V128C512 92.63 483.4 64 448 64zM160 368H80C71.13 368 64 360.9 64 352v-16C64 327.1 71.13 320 80 320H160V368zM288 352c0 8.875-7.125 16-16 16H192V320h80c8.875 0 16 7.125 16 16V352zM448 224c0 17.62-14.38 32-32 32H96C78.38 256 64 241.6 64 224V160c0-17.62 14.38-32 32-32h320c17.62 0 32 14.38 32 32V224z"));
            IconElement.SetWidth(tabItem, 15);
            IconElement.SetHeight(tabItem, 15);
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
            Growl.Success("Successfully closed file");
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
            Growl.Success("Successfully opened file");
        }

        public static void saveFile()
        {
            if (getTextEditor() == null)
                return;
            TabItem currentTabItem = tabControl.SelectedItem as TabItem;
            if (currentTabItem.ToolTip != null && currentTabItem.ToolTip.ToString() != "")
            {
                getTextEditor().Save(currentTabItem.ToolTip.ToString());
                Growl.Success("Successfully saved file");
                return;
            }
            saveAsFile();
        }

        public static void saveAsFile()
        {
            if (getTextEditor() == null)
                return;
            SaveFileDialog saveDialog = new SaveFileDialog() { FileName = ((TabItem)tabControl.SelectedItem).Header.ToString() };
            if (saveDialog.ShowDialog() != DialogResult.OK)
                return;
            getTextEditor().Save(saveDialog.FileName);
            TabItem currentTabItem = tabControl.SelectedItem as TabItem;
            currentTabItem.ToolTip = saveDialog.FileName;
            currentTabItem.Header = new FileInfo(saveDialog.FileName).Name;
            Growl.Success("Successfully saved file");
        }
    }
}

using GlitchCode.Managers;
using HandyControl.Controls;
using System.Collections.Generic;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;
using Window = HandyControl.Controls.Window;

namespace GlitchCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static List<FrameworkElement> TitleBarItems = new List<FrameworkElement>();
        public static List<FrameworkElement> Items = new List<FrameworkElement>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (FrameworkElement item in TitleBar.Children)
            {
                TitleBarItems.Add(item);
            }
            foreach (FrameworkElement item in MainBorder.Children)
            {
                Items.Add(item);
            }
            FileManager.createWelcomeTabItem();
            DiscordRPCManager.Initializate((TabControl.SelectedItem as TabItem).Header.ToString());
            FuncionalitiesManager.LoadAll();
        }
    }
}

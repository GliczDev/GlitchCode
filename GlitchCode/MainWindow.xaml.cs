using GlitchCode.Managers;
using HandyControl.Controls;
using HandyControl.Tools;
using System.Windows;
using Window = HandyControl.Controls.Window;

namespace GlitchCode
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ApplyBackdropMaterial = OSVersionHelper.IsWindows11_OrGreater && Properties.Settings.Default.EnableMicaEffect && Properties.Settings.Default.Theme.Equals("Sync");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FileManager.createWelcomeTabItem();
            DiscordRPCManager.Initializate((TabControl.SelectedItem as TabItem).Header.ToString());
            FuncionalitiesManager.LoadAll();
        }
    }
}
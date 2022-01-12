using GlitchCode.Managers;
using GlitchCode.Pages;
using HandyControl.Tools;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GlitchCode.Functionalities.Func
{
    class WindowFunc : IFuncionality
    {
        static TextBlock TitleText = WindowManager.getMainWindow().TitleText;
        static Border Border = WindowManager.getMainWindow().Border;
        static Button settingsButton = WindowManager.getMainWindow().SettingsButton;

        public void onEnable()
        {
            WindowManager.getMainWindow().SizeChanged += MainWindow_SizeChanged;
            settingsButton.Click += settingsButton_Click;
            if (OSVersionHelper.IsWindows11_OrGreater)
                Border.Background = Brushes.Transparent;
            updateLocation();
        }

        private static void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow.Show(new SettingsPage());
        }

        void updateLocation()
        {
            Canvas.SetTop(TitleText, 4);
            Canvas.SetLeft(TitleText, (WindowManager.getMainWindow().ActualWidth / 2) - (TitleText.ActualWidth / 2));
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            updateLocation();
        }
    }
}

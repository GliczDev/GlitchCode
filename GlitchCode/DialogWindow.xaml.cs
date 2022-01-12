using HandyControl.Tools;
using System.Windows.Controls;
using Window = HandyControl.Controls.Window;

namespace GlitchCode
{
    /// <summary>
    /// Logika interakcji dla klasy DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public static DialogWindow Show(UserControl userControl)
        {
            DialogWindow dialogWindow = new DialogWindow()
            {
                ApplyBackdropMaterial = OSVersionHelper.IsWindows11_OrGreater && Properties.Settings.Default.EnableMicaEffect && Properties.Settings.Default.Theme.Equals("Sync"),
                Content = userControl
            };
            dialogWindow.Show();
            return dialogWindow;
        }
    }
}

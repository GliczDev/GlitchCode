using GlitchCode.Functionalities.Func;
using System.Windows.Media;
using UserControl = System.Windows.Controls.UserControl;

namespace GlitchCode.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : UserControl
    {
        static BrushConverter converter = new BrushConverter();

        public SettingsPage()
        {
            InitializeComponent();
            new SettingsDialogFunc(this);
        }
    }
}

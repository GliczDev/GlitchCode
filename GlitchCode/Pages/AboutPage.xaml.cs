using System.Windows.Controls;

namespace GlitchCode.Pages
{
    /// <summary>
    /// Logika interakcji dla klasy AboutPage.xaml
    /// </summary>
    public partial class AboutPage : UserControl
    {
        public AboutPage()
        {
            InitializeComponent();
            Version.Text += System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }
}

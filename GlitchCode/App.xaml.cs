using System;
using System.Windows;

namespace GlitchCode
{
    /// <summary>
    /// Logika interakcji dla klasy App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static string startupFile;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
                startupFile = args[1];
        }
    }
}

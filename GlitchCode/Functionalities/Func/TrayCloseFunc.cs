using GlitchCode.Managers;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace GlitchCode.Functionalities.Func
{
    class TrayCloseFunc : IFuncionality
    {
        static Bitmap bitMap = new Bitmap(@"C:\Users\emtel\OneDrive\GlitchCode\GlitchCode\Images\GlitchCode.png");
        NotifyIcon notifyIcon = new NotifyIcon() { Icon = Icon.FromHandle(bitMap.GetHicon()), Text = "GlitchCode", ContextMenuStrip = new ContextMenuStrip() };

        public void Load()
        {
            App.Current.MainWindow.Closing += MainWindow_Closing;
            notifyIcon.MouseClick += notifyIcon_Click;
            notifyIcon.ContextMenuStrip.Items.Add("Close", null, closeButton_Click);
        }

        void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (Properties.Settings.Default.TrayClose != true)
                return;
            e.Cancel = true;
            App.Current.MainWindow.Hide();
            DiscordRPCManager.Deinitialize();
            notifyIcon.Visible = true;
        }

        void notifyIcon_Click(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            App.Current.MainWindow.Show();
            DiscordRPCManager.Initializate();
            notifyIcon.Visible = false;
        }

        void closeButton_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}

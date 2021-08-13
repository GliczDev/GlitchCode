using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GlitchCode
{
    /// <summary>
    /// Logika interakcji dla klasy SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        public bool IsYes { get; private set; }

        public SettingsWindow()
        {
            InitializeComponent();
            showLineNumbers.IsChecked = Properties.Settings.Default.showLineNumbers;
            showTabs.IsChecked = Properties.Settings.Default.showTabs;
            _SaveB = SaveB;
            _CancelB = CancelB;
            _ShowLineNumbersCB = showLineNumbers;
            _ShowTabsCB = showTabs;
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            saveSettings();
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.15));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        public Button _SaveB;
        public Button _CancelB;
        public CheckBox _ShowLineNumbersCB;
        public CheckBox _ShowTabsCB;

        private void closeButton_Click(object sender, MouseButtonEventArgs e)
        {
            IsYes = false;
            this.Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
                DragMove();
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsYes = false;
            this.Close();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            IsYes = true;
            this.Close();
        }

        void saveSettings()
        {
            Properties.Settings.Default.showLineNumbers = showLineNumbers.IsChecked.GetValueOrDefault();
            Properties.Settings.Default.showTabs = showTabs.IsChecked.GetValueOrDefault();
            Properties.Settings.Default.Save();
        }
    }
}

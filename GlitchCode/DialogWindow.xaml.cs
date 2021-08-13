using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public bool IsYes { get; private set; }

        public DialogWindow(string Text)
        {
            InitializeComponent();
            this.TextLabel.Content = Text;
            _YesB = YesB;
            _NoB = NoB;
        }

        public Button _YesB;
        public Button _NoB;

        private void closeButton_Click(object sender, MouseButtonEventArgs e)
        {
            IsYes = false;
            Close();
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
                DragMove();
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            IsYes = true;
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Closing -= Window_Closing;
            e.Cancel = true;
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.15));
            anim.Completed += (s, _) => this.Close();
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            IsYes = false;
            Close();
        }
    }
}

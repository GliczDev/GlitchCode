using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace GlitchCode
{
    /// <summary>
    /// Logika interakcji dla klasy Window1.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, MouseButtonEventArgs e)
        {
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
            this.DialogResult = true;
        }

        public static bool showDialog(string Text)
        {
            DialogWindow dialog = new DialogWindow();
            dialog.TextLabel.Content = Text;
            return (bool)dialog.ShowDialog();
        }
    }
}

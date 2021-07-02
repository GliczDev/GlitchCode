using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace GlitchCode.Api
{
    public class Elements
    {
        public static Border Border()
        {
            return MainWindow._Border;
        }
        public static ICSharpCode.AvalonEdit.TextEditor TextEditor()
        {
            return MainWindow._TextEditor;
        }
    }
}

using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace GlitchCode
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string allExtensions = "All files|*.*|ASP/XHTML|*.asp;*.aspx;*.asax;*.asmx;*.ascx;*.master|Boo|*.boo|Coco|*.atg|C++|*.c;*.h;*.cc;*.cpp;*.hpp|C#|*.cs|CSS|*.css|BAT|*.bat;*.dos|F#|*.fs|HSLS|*.fx|HTML|*.htm;*.html|INI|*.cfg;*.conf;*.ini;*.iss|Java|*.java|JavaScript|*.js|LOG|*.log|MarkDown|*.md|Pascal|*.pas|Patch|*.patch;*.diff|PHP|*.php|PLSQL|*.plsql|PowerShell|*.ps1;*.psm1;*.psd1|Python|*.py;*.pyw|Ruby|*.rb|Scheme|*.sls;*.sps;*.ss;*.scm|Squirrel|*.nut|Tex|*.tex|TSQL|*.sql|TXT|*.txt|VB|*.vb|VTL|*.vtl;*.vm|XML|*.xml;*.xsl;*.xslt;*.xsd;*.manifest;*.config;*.addin;*.xshd;*.wxs;*.wxi;*.wxl;*.proj;*.csproj;*.vbproj;*.ilproj;*.booproj;*.build;*.xfrm;*.targets;*.xaml;*.xpt;*.xft;*.map;*.wsdl;*.disco;*.ps1xml;*.nuspec";

        public MainWindow()
        {
            InitializeComponent();
        }

        private void closeButton_Click(object sender, MouseButtonEventArgs e)
        {
            Close();
        }
        private void maximazeButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (MainWindow.GetWindow(this).WindowState == WindowState.Maximized)
            {
                MainWindow.GetWindow(this).WindowState = WindowState.Normal;
            }
            else
            {
                MainWindow.GetWindow(this).WindowState = WindowState.Maximized;
            }
        }
        private void minimizeButton_Click(object sender, MouseButtonEventArgs e)
        {
            MainWindow.GetWindow(this).WindowState = WindowState.Minimized;
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);
            this.DragMove();
        }

        //File (MenuStrip)
        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            AvalonEdit.Text = "";
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = allExtensions;
            if (openDialog.ShowDialog() == true)
                AvalonEdit.Text = File.ReadAllText(openDialog.FileName);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = allExtensions;
            if (saveDialog.ShowDialog() == true)
                AvalonEdit.Save(saveDialog.FileName);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            AvalonEdit.Text = "";
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //ContextMenu/Edit (MenuStrip)
        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            AvalonEdit.Undo();
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            AvalonEdit.Redo();
        }

        private void cutButton_Click(object sender, RoutedEventArgs e)
        {
            AvalonEdit.Cut();
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            AvalonEdit.Copy();
        }

        private void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            AvalonEdit.Paste();
        }

        private void selectAllButton_Click(object sender, RoutedEventArgs e)
        {
            AvalonEdit.SelectAll();
        }
    }
}

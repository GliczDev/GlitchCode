using DiscordRPC;
using ICSharpCode.AvalonEdit.Highlighting;
using ICSharpCode.AvalonEdit.Highlighting.Xshd;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Xml;
using Button = DiscordRPC.Button;

namespace GlitchCode
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string allExtensions = "All files|*.*|ASP/XHTML|*.asp;*.aspx;*.asax;*.asmx;*.ascx;*.master|Boo|*.boo|Coco|*.atg|C++|*.c;*.h;*.cc;*.cpp;*.hpp|C#|*.cs|CSS|*.css|BAT|*.bat;*.dos|F#|*.fs|HSLS|*.fx|HTML|*.htm;*.html|INI|*.cfg;*.conf;*.ini;*.iss|Java|*.java|JavaScript|*.js|LOG|*.log|MarkDown|*.md|Pascal|*.pas|Patch|*.patch;*.diff|PHP|*.php|PLSQL|*.plsql|PowerShell|*.ps1;*.psm1;*.psd1|Python|*.py;*.pyw|Ruby|*.rb|Scheme|*.sls;*.sps;*.ss;*.scm|Squirrel|*.nut|Tex|*.tex|TSQL|*.sql|TXT|*.txt|VB|*.vb|VTL|*.vtl;*.vm|XML|*.xml;*.xsl;*.xslt;*.xsd;*.manifest;*.config;*.addin;*.xshd;*.wxs;*.wxi;*.wxl;*.proj;*.csproj;*.vbproj;*.ilproj;*.booproj;*.build;*.xfrm;*.targets;*.xaml;*.xpt;*.xft;*.map;*.wsdl;*.disco;*.ps1xml;*.nuspec";

        DiscordRpcClient client = new DiscordRpcClient("854763230080794664");
        public static Border _Border;
        public static ICSharpCode.AvalonEdit.TextEditor _TextEditor;
        public static Menu _MenuStrip;
        public static ContextMenu _ContextMenu;

        public MainWindow()
        {
            InitializeComponent();
            _Border = Border;
            _TextEditor = AvalonEdit;
            _MenuStrip = MenuStrip;
            _ContextMenu = ContextMenu;
            AvalonEdit.Options.EnableHyperlinks = false;
            if (App.startupFile != null)
                AvalonEdit.Load(App.startupFile);
            client.Initialize();
            client.SetPresence(new RichPresence()
            {
                Details = "Editing code...",
                Timestamps = Timestamps.Now,
                Buttons = new Button[]
                {
                        new Button()
                        {
                            Label = "Get GlitchCode",
                            Url = "https://github.com/MichixYT/GlitchCode"
                        }
                },
                Assets = new Assets()
                {
                    LargeImageKey = "glitchcode",
                    LargeImageText = "GlitchCode"
                }
            });
            loadAddons();
            try
            {
                XmlTextReader reader = new XmlTextReader(new StreamReader(@"D:\Michał\Visual Studio\GlitchCode\GlitchCode\Themes\VSTheme.xshd"));
                AvalonEdit.SyntaxHighlighting = HighlightingLoader.Load(reader, HighlightingManager.Instance);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        void loadAddons()
        {
            try
            {
                var appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                Directory.CreateDirectory(Path.Combine(appdataFolder, "GlitchCode", "Addons"));
                var addonFolder = Path.Combine(appdataFolder, "GlitchCode", "Addons");
                foreach (var dllFile in Directory.EnumerateFiles(addonFolder, "*.dll", SearchOption.AllDirectories))
                {
                    var assembly = Assembly.LoadFrom(dllFile);
                    var addonTypes = AppDomain.CurrentDomain.GetAssemblies()
                        .SelectMany(s => s.GetTypes())
                        .Where(p => typeof(IGlitchCodeAddon).IsAssignableFrom(p))
                        .Where(w => w.IsClass && !w.IsAbstract);
                    foreach (var addonType in addonTypes)
                    {
                        var addonInstance = (IGlitchCodeAddon)Activator.CreateInstance(addonType);
                        addonInstance.Run();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        bool wantToContinue(string Message)
        {
            if (DialogWindow.showDialog(Message))
            {
                return true;
            }
            return false;
        }

        async void publishCode(string Code)
        {
            if (string.IsNullOrWhiteSpace(Code))
                Code = " ";
            CookieContainer cookies = new CookieContainer();
            HttpClientHandler handler = new HttpClientHandler();
            handler.CookieContainer = cookies;
            using (HttpClient client = new HttpClient(handler))
            {
                using (HttpResponseMessage response = client.GetAsync("https://code.skript.pl").Result)
                {
                    HttpHeaders headers = response.Headers;
                    HttpContent content = response.Content;
                    string myContent = content.ReadAsStringAsync().Result;
                    Regex regex = new Regex("name='csrf-token' content='(.*)'");
                    var otherLang = regex.Match(myContent).Groups[1];
                    var values = new Dictionary<string, string>
                    {
                        { "lang", "text" },
                        { "content", Code },
                        { "_token", otherLang.ToString() }
                    };
                    var content2 = new FormUrlEncodedContent(values);
                    var response2 = await client.PostAsync("https://code.skript.pl/create", content2);
                    var responseString = response2.RequestMessage.RequestUri;
                    System.Diagnostics.Process.Start(responseString.ToString());
                }
            }
        }

        private void closeButton_Click(object sender, MouseButtonEventArgs e)
        {
            if (wantToContinue("Are you sure, to close current file?"))
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
            if (e.LeftButton.Equals(MouseButtonState.Pressed))
                DragMove();
        }

        //File (MenuStrip)
        private void newButton_Click(object sender, RoutedEventArgs e)
        {
            if (wantToContinue("Are you sure, to close current file?"))
                AvalonEdit.Text = "";
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = allExtensions;
            if (openDialog.ShowDialog() == true)
                if (wantToContinue("Are you sure, to close current file?"))
                    AvalonEdit.Load(openDialog.FileName);
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = allExtensions;
            if (saveDialog.ShowDialog() == true)
                AvalonEdit.Save(saveDialog.FileName);
        }

        private void publishCodeButton_Click(object sender, RoutedEventArgs e)
        {
            publishCode(AvalonEdit.Text);
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            if (wantToContinue("Are you sure, to close current file?"))
                AvalonEdit.Text = "";
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            if (wantToContinue("Are you sure, to close current file?"))
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
        
        //Shortcuts
        private void shortCutNewButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (wantToContinue("Are you sure, to close current file?"))
                AvalonEdit.Text = "";
        }

        private void shortCutOpenButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = allExtensions;
            if (openDialog.ShowDialog() == true)
                if (wantToContinue("Are you sure, to close current file?"))
                    AvalonEdit.Load(openDialog.FileName);
        }

        private void shortCutSaveButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = allExtensions;
            if (saveDialog.ShowDialog() == true)
                AvalonEdit.Save(saveDialog.FileName);
        }

        private void shortCutPublishCodeButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            publishCode(AvalonEdit.Text);
        }

        private void shortCutCloseButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            if (wantToContinue("Are you sure, to close current file?"))
                AvalonEdit.Text = "";
        }
    }
}
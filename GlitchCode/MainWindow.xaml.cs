using DiscordRPC;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
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
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
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
        public static Style _TextEditorStyle;
        public static ICSharpCode.AvalonEdit.TextEditor _CurrentTextEditor;
        public static Menu _MenuStrip;
        public static ContextMenu _ContextMenu;

        public MainWindow()
        {
            InitializeComponent();
            StartPage startPage = new StartPage();
            startPage.NewFile.PreviewMouseDown += this.NewFile_PreviewMouseOver;
            startPage.OpenFile.PreviewMouseDown += this.OpenFile_PreviewMouseOver;
            ContentControl contentControl = new ContentControl()
            {
                Content = startPage
            };
            TabItem tabItem = new TabItem()
            {
                Header = "Welcome!",
                Content = contentControl
            };
            TabControl.Items.Add(tabItem);
            TabControl.SelectedItem = tabItem;
            _Border = Border;
            _TextEditorStyle = FindResource("TextEditor") as Style;
            _MenuStrip = MenuStrip;
            _ContextMenu = FindResource("ContextMenu1") as ContextMenu;
            if (App.startupFile != null)
            {
                newFile();
                GetCurrentTextBox().Load(App.startupFile);
                TabItem currentTabItem = TabControl.SelectedItem as TabItem;
                currentTabItem.ToolTip = App.startupFile;
                currentTabItem.Header = Path.GetFileName(App.startupFile);
            }
            loadDiscordRPC();
            loadAddons();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (wantToContinue("Are you sure, to close current files?"))
            {
                Closing -= Window_Closing;
                e.Cancel = true;
                var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.15));
                anim.Completed += (s, _) => this.Close();
                this.BeginAnimation(UIElement.OpacityProperty, anim);
            } 
            else
            {
                e.Cancel = true;
            }
        }

        void loadDiscordRPC()
        {
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
        }

        void newFile()
        {
            if (GetCurrentTextBox() == null)
                TabControl.Items.Remove(TabControl.SelectedItem);
            ICSharpCode.AvalonEdit.TextEditor textEditor = new ICSharpCode.AvalonEdit.TextEditor() 
            {
                ContextMenu = FindResource("ContextMenu1") as ContextMenu,
                Style = FindResource("TextEditor") as Style
            };
            TabItem tabItem = new TabItem()
            {
                Header = "Untitled-" + (TabControl.Items.Count + 1),
                Content = textEditor
            };
            TabControl.Items.Add(tabItem);
            TabControl.SelectedItem = tabItem;
        }

        void openFile()
        {
            OpenFileDialog openDialog = new OpenFileDialog();
            openDialog.Filter = allExtensions;
            if (openDialog.ShowDialog() == false)
                return;
            if (GetCurrentTextBox() == null)
                TabControl.Items.Remove(TabControl.SelectedItem);
            newFile();
            GetCurrentTextBox().Load(openDialog.FileName);
            TabItem currentTabItem = TabControl.SelectedItem as TabItem;
            currentTabItem.ToolTip = openDialog.FileName;
            currentTabItem.Header = openDialog.SafeFileName;
        }

        void saveFile()
        {
            if (GetCurrentTextBox() == null)
                return;
            TabItem currentTabItem = TabControl.SelectedItem as TabItem;
#pragma warning disable CS0252
            if (currentTabItem.ToolTip != null && currentTabItem.ToolTip != "")
#pragma warning restore CS0252
            {
                GetCurrentTextBox().Save(currentTabItem.ToolTip.ToString());
                return;
            }
            saveAsFile();
        }

        void saveAsFile()
        {
            if (GetCurrentTextBox() == null)
                return;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.Filter = allExtensions;
            if (saveDialog.ShowDialog() == false)
                return;
            GetCurrentTextBox().Save(saveDialog.FileName);
            TabItem currentTabItem = TabControl.SelectedItem as TabItem;
            currentTabItem.ToolTip = saveDialog.FileName;
            currentTabItem.Header = saveDialog.SafeFileName;
        }

        void closeFile()
        {
            if (GetCurrentTextBox() != null)
                if (!wantToContinue("Are you sure, to close current file?"))
                    return;
            TabControl.Items.Remove(TabControl.SelectedItem);
            GC.Collect();
            if (TabControl.Items.Count == 0)
                newFile();
        }

        void closeAllFiles()
        {
            if (GetCurrentTextBox() != null)
                if (!wantToContinue("Are you sure, to close current files?"))
                    return;
            TabControl.Items.Clear();
            GC.Collect();
            newFile();
        }

        ICSharpCode.AvalonEdit.TextEditor GetCurrentTextBox()
        {
            ICSharpCode.AvalonEdit.TextEditor textEditor = null;
            TabItem tp = TabControl.SelectedItem as TabItem;
            if (tp != null)
                textEditor = tp.Content as ICSharpCode.AvalonEdit.TextEditor;
            return textEditor;
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
            var dialogWindow = new DialogWindow(Message);
            dialogWindow.ShowDialog();
            if (dialogWindow.IsYes)
                return true;
            return false;
        }

        async void publishCode()
        {
            if (GetCurrentTextBox() == null)
                return;
            string Code = GetCurrentTextBox().Text;
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
            this.Close();
        }
        private void maximazeButton_Click(object sender, MouseButtonEventArgs e)
        {
            ImageBrush ib = new ImageBrush();
            if (MainWindow.GetWindow(this).WindowState == WindowState.Maximized)
            {
                ib.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/Images/maximaze1.png"));
                MainWindow.GetWindow(this).WindowState = WindowState.Normal;
            }
            else
            {
                
                ib.ImageSource = new BitmapImage(new Uri($"pack://application:,,,/Images/maximaze2.png"));
                MainWindow.GetWindow(this).WindowState = WindowState.Maximized;
            }
            maximazeButton.Fill = ib;
        }
        
        private void minimizeButton_Click(object sender, MouseButtonEventArgs e)
        {
            var anim = new DoubleAnimation(0, (Duration)TimeSpan.FromSeconds(0.15));
            anim.Completed += (s, _) => MainWindow.GetWindow(this).WindowState = WindowState.Minimized;
            this.BeginAnimation(UIElement.OpacityProperty, anim);
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
            newFile();
        }

        private void openButton_Click(object sender, RoutedEventArgs e)
        {
            openFile();
        }

        private void saveButton_Click(object sender, RoutedEventArgs e)
        {
            saveFile();
        }

        private void saveAsButton_Click(object sender, RoutedEventArgs e)
        {
            saveAsFile();
        }

        private void publishCodeButton_Click(object sender, RoutedEventArgs e)
        {
            publishCode();
        }

        private void closeButton_Click(object sender, RoutedEventArgs e)
        {
            closeFile();
        }

        private void closeAllButton_Click(object sender, RoutedEventArgs e)
        {
            closeAllFiles();
        }

        private void exitButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        //ContextMenu/Edit (MenuStrip)
        private void undoButton_Click(object sender, RoutedEventArgs e)
        {
            GetCurrentTextBox().Undo();
        }

        private void redoButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentTextBox() == null)
                return;
            GetCurrentTextBox().Redo();
        }

        private void cutButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentTextBox() == null)
                return;
            GetCurrentTextBox().Cut();
        }

        private void copyButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentTextBox() == null)
                return;
            GetCurrentTextBox().Copy();
        }

        private void pasteButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentTextBox() == null)
                return;
            GetCurrentTextBox().Paste();
        }

        private void selectAllButton_Click(object sender, RoutedEventArgs e)
        {
            if (GetCurrentTextBox() == null)
                return;
            GetCurrentTextBox().SelectAll();
        }
        
        //Shortcuts
        private void shortCutNewButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            newFile();
        }

        private void shortCutOpenButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            openFile();
        }

        private void shortCutSaveButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            saveFile();
        }

        private void shortCutSaveAsButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            saveAsFile();
        }

        private void shortCutCloseButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            closeFile();
        }

        private void shortCutCloseAllButton_Click(object sender, ExecutedRoutedEventArgs e)
        {
            closeAllFiles();
        }

        private void settingsButton_Click(object sender, RoutedEventArgs e)
        {
            var settingsWindow = new SettingsWindow();
            settingsWindow.ShowDialog();
            if (settingsWindow.IsYes)
                loadSettings();
        }

        private void Window_Activated(object sender, EventArgs e)
        {
            var anim = new DoubleAnimation(1, (Duration)TimeSpan.FromSeconds(0.15));
            this.BeginAnimation(UIElement.OpacityProperty, anim);
        }

        void loadSettings()
        {
            if (GetCurrentTextBox() == null)
                return;
            GetCurrentTextBox().ShowLineNumbers = Properties.Settings.Default.showLineNumbers;
            GetCurrentTextBox().Options.ShowTabs = Properties.Settings.Default.showTabs;
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.Source is TabControl)
            {
                if (GetCurrentTextBox() == null)
                    return;
                _CurrentTextEditor = GetCurrentTextBox();
                loadSettings();
            }
        }

        private void NewFile_PreviewMouseOver(object sender, MouseButtonEventArgs e)
        {
            newFile();
        }
        private void OpenFile_PreviewMouseOver(object sender, MouseButtonEventArgs e)
        {
            openFile();
        }
    } 
}
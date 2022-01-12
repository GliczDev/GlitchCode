using GlitchCode.Addons;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using MessageBox = HandyControl.Controls.MessageBox;

namespace GlitchCode.Functionalities.Func
{
    class AddonLoaderFunc : IFuncionality
    {
        public void onEnable()
        {
            var appdataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            var directory = Directory.CreateDirectory(Path.Combine(appdataFolder, "GlitchCode", "Addons"));
            directory.Attributes = FileAttributes.Directory | FileAttributes.Hidden;
            var addonFolder = Path.Combine(appdataFolder, "GlitchCode", "Addons");
            foreach (var dllFile in Directory.EnumerateFiles(addonFolder, "*.dll", SearchOption.AllDirectories))
            {
                var assembly = Assembly.LoadFrom(dllFile);
            }
            try
            {
                var addonTypes = AppDomain.CurrentDomain.GetAssemblies()
                    .SelectMany(s => s.GetTypes())
                    .Where(p => typeof(IGlitchCodeAddon).IsAssignableFrom(p))
                    .Where(w => w.IsClass && !w.IsAbstract);
                foreach (var addonType in addonTypes)
                {
                    var addonInstance = (IGlitchCodeAddon)Activator.CreateInstance(addonType);
                    addonInstance.onEnable();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error has been occurred while enabling one of addons.\n\nError message:\n" + ex.Message, "GlitchCode", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(0);
            }
        }
    }
}

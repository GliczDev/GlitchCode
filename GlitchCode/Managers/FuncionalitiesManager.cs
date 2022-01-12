using GlitchCode.Functionalities;
using System;
using System.Linq;
using System.Reflection;

namespace GlitchCode.Managers
{
    public static class FuncionalitiesManager
    {
        public static void LoadAll()
        {
            var types = Assembly
                .GetExecutingAssembly()
                .GetTypes()
                .Where(t => t.Namespace.StartsWith("GlitchCode.Functionalities.Func"))
                .Where(t => typeof(IFuncionality).IsAssignableFrom(t));
            foreach (var type in types)
            {
                var funcInstance = (IFuncionality)Activator.CreateInstance(type);
                funcInstance.onEnable();
            }
        }
    }
}

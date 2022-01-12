using GlitchCode.SyntaxHighlighting;
using System.Collections.Generic;

namespace GlitchCode.Managers
{
    public static class SyntaxHighlightingManager
    {
        static Dictionary<SyntaxInfo, string> Syntaxes = new Dictionary<SyntaxInfo, string>();

        public static void registerSyntax(SyntaxInfo syntaxInfo, string syntax)
        {
            Syntaxes.Add(syntaxInfo, syntax);
        }

        public static string getSyntaxFromName(string name)
        {
            return Syntaxes[SyntaxInfo.getFromName(name)];
        }
    }
}

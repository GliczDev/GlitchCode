using System.Collections.Generic;

namespace GlitchCode.SyntaxHighlighting
{
    public class SyntaxInfo
    {
        public string _Name { get; private set; }
        public string[] _Extensions { get; private set; }
        static List<SyntaxInfo> syntaxInfos = new List<SyntaxInfo>();

        public SyntaxInfo(string Name, string[] Extensions)
        {
            _Name = Name;
            _Extensions = Extensions;
            syntaxInfos.Add(this);
        }

        public static SyntaxInfo getFromName(string name)
        {
            return syntaxInfos.Find(syntaxInfo => syntaxInfo._Name == name);
        }
    }
}

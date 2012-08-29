//Add by onetwogoo
using System.Collections.Generic;
using ColorCode.Common;

namespace ColorCode.Compilation.Languages
{
    class Pascal : ILanguage
    {
        public string Id
        {
            get { return LanguageId.Pascal; }
        }

        public string Name
        {
            get { return "Pascal"; }
        }

        public string FirstLinePattern
        {
            get { return null; }
        }

        public string CssClassName
        {
            get { return "pascal"; }
        }

        public IList<LanguageRule> Rules
        {
            get
            {
                return new List<LanguageRule>(){
                    new LanguageRule(
                        @"(//.*?)\r?$",
                        new Dictionary<int,string>(){
                            {1,ScopeName.Comment}
                        }),
                    new LanguageRule(
                        @"\{([^}]|[\r\n])*\}",
                        new Dictionary<int,string>(){
                            {0,ScopeName.Comment}
                        }),
                    new LanguageRule(
                        @"''|'[^\r\n]*?(?<!')'",
                        new Dictionary<int,string>(){
                            {0,ScopeName.String}
                        }),
                    new LanguageRule(
                        @"(?i)\b(abs|absolute|abstract|and|arctan|array|as|asm|assembler|at|automated|begin|boolean|byte|case|cdecl|char|chr|class|comp|const|constructor|contains|cos|default|destructor|destructoruses|dispid|dispinterface|div|do|double|downto|dynamic|else|end|except|exp|export|exports|extended|external|false|far|file|finalization|finally|for|forward|function|goto|if|implementation|implements|in|index|inherited|initialization|inline|int64|integer|interface|is|label|library|ln|longint|maxint|maxlongint|message|mod|name|near|nil|nodefault|not|object|of|on|or|ord|out|overload|override|package|packed|pascal|pred|private|procedure|program|property|protected|public|published|qword|raise|read|readln|readonly|real|record|register|reintroduce|repeat|requires|resident|resourcestring|round|safecall|set|shl|shortint|shr|sin|single|sqr|sqrt|stdcall|stored|string|succ|then|threadvar|to|true|trunc|try|type|unit|until|uses|var|virtual|while|with|word|write|writeln|writeonly|xor)\b",
                        new Dictionary<int,string>(){
                            {0,ScopeName.Keyword}
                        })
                };
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}

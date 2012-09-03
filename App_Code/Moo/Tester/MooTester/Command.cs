using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Moo.Tester.MooTester
{
    /// <summary>
    /// 各个命令行
    /// </summary>
    public static class Command
    {
        static IDictionary<string, IDictionary<string, string>> commands = new Dictionary<string, IDictionary<string, string>>()
        {
            {
                "c++",
                new Dictionary<string,string>()
                {
                    {"src2exe","g++ -Wall -O2 -std=c++0x -o {Execute} {E.exeE} {Source} {S.cppS}"},
                    {"src2obj","g++ -Wall -O2 -std=c++0x -c -o {Execute} {E.oE} {Source} {S.cppS}"},
                    {"obj2exe","g++ -Wall -O2 -std=c++0x -o {Execute} {E.exeE} {Object}"}
                }
            },
            {
                "c",
                new Dictionary<string,string>()
                {
                    {"src2exe","gcc -Wall -O2 -o {Execute} {E.exeE} {Source} {S.cS}"},
                    {"src2obj","gcc -Wall -O2 -c -o {Execute} {E.oE} {Source} {S.cS}"},
                    {"obj2exe","gcc -Wall -O2 -o {Execute} {E.exeE} {Object}"}
                }
            },
            {
                "pascal",
                new Dictionary<string,string>()
                {
                    {"src2exe","ppcrossx64 -o{Execute} {E.exeE} {Source} {S.pasS}"}
                }
            }
        };

        public static string GetCommand(string language, string type)
        {
            if (commands.ContainsKey(language))
            {
                IDictionary<string, string> dic = commands[language];
                if (dic.ContainsKey(type))
                {
                    return dic[type];
                }
                else
                {
                    throw new MooTesterException()
                    {
                        Result = new TestResult()
                        {
                            Score = 0,
                            Info = string.Format(Resources.Moo.MooTester_UnsupportedLanguageOperation, language, type)
                        }
                    };
                }
            }
            else
            {
                throw new MooTesterException()
                {
                    Result = new TestResult()
                    {
                        Score = 0,
                        Info = string.Format(Resources.Moo.MooTester_UnsupportedLanguage, language)
                    }
                };
            }
        }
    }
}
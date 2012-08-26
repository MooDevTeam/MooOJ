using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Moo.DB;
namespace Moo.Tester
{
    public interface ITester
    {
        TestResult TestTranditional(string source, string language, IEnumerable<TranditionalTestCase> cases);
        TestResult TestSpecialJudged(string source, string language, IEnumerable<SpecialJudgedTestCase> cases);
    }
}
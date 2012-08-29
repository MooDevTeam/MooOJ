using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using Moo.Tester;
using Moo.DB;
using Moo.Utility;
namespace Moo.Tester.MooTester
{
    public class Tester : ITester
    {
        public TestResult TestTranditional(string source, string language, IEnumerable<TranditionalTestCase> cases)
        {
            int failureCount = 0;
            while (failureCount < 5)
            {
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        socket.Connect(Resources.Moo.MooTester_TesterIP, int.Parse(Resources.Moo.MooTester_TesterPort));
                        return InnerTestTranditional(socket, source, language, cases);
                    }
                }
                catch (SocketException)
                {
                    failureCount++;
                    Thread.Sleep(1000);
                }
            }
            Logger.Log("Continues SocketException With MooTester");
            return new TestResult()
            {
                Score = 0,
                Info = Resources.Moo.MooTester_NetworkError
            };
        }

        public TestResult TestSpecialJudged(string source, string language, IEnumerable<SpecialJudgedTestCase> cases)
        {
            int failureCount = 0;
            while (failureCount < 5)
            {
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        socket.Connect(Resources.Moo.MooTester_TesterIP, int.Parse(Resources.Moo.MooTester_TesterPort));
                        return InnerTestSpecialJudged(socket, source, language, cases);
                    }
                }
                catch (SocketException)
                {
                    failureCount++;
                    Thread.Sleep(1000);
                }
            }
            Logger.Log("Continues SocketException With MooTester");
            return new TestResult()
            {
                Score = 0,
                Info = Resources.Moo.MooTester_NetworkError
            };
        }

        TestResult compile(Socket socket, string source, string language, out string execFilePath)
        {
            execFilePath = null;

            string compileCommand;
            switch (language)
            {
                case "c++":
                    compileCommand = "g++ -o {Execute} {Source} {.cpp}";
                    break;
                case "c":
                    compileCommand = "gcc -o {Execute} {Source} {.c}";
                    break;
                default:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = Resources.Moo.MooTester_LanguageNotSupported
                    };
            }

            socket.Send(new Message()
            {
                Type = Message.MessageType.Compile,
                Content = new CompileIn()
                {
                    Command = compileCommand,
                    Code = source,
                    Memory = int.Parse(Resources.Moo.MooTester_CompileMemory),
                    Time = int.Parse(Resources.Moo.MooTester_CompileTime)
                }
            }.ToBytes());

            Out compileResult = new Out(socket);
            switch (compileResult.Type)
            {
                case Out.ResultType.Success:
                    execFilePath = compileResult.Message;
                    return null;
                case Out.ResultType.TimeLimitExceeded:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = Resources.Moo.MooTester_CompilerTLE
                    };
                case Out.ResultType.RuntimeError:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = string.Format(Resources.Moo.MooTester_CompilerRE, compileResult.Message)
                    };
                case Out.ResultType.MemoryLimitExceeded:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = Resources.Moo.MooTester_CompilerMLE
                    };
                default:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = Resources.Moo.MooTester_CompilerUndefinedError
                    };
            }
        }

        TestResult InnerTestTranditional(Socket socket, string source, string language, IEnumerable<TranditionalTestCase> cases)
        {
            string execFilePath;
            TestResult compileResult = compile(socket, source, language, out execFilePath);
            if (compileResult != null) return compileResult;

            int score = 0;
            StringBuilder sb = new StringBuilder(Resources.Moo.MooTester_CompilerSuccess).AppendLine().AppendLine();
            foreach (TranditionalTestCase testCase in cases)
            {
                sb.AppendFormat(Resources.Moo.MooTester_TestCaseX, testCase.ID);

                socket.Send(new Message()
                {
                    Type = Message.MessageType.Test,
                    Content = new TestIn()
                    {
                        CmpPath = Resources.Moo.MooTester_TranditionalJudger,
                        ExecPath = execFilePath,
                        Memory = testCase.MemoryLimit,
                        Time = testCase.TimeLimit,
                        Input = testCase.Input,
                        Output = testCase.Answer
                    }
                }.ToBytes());
                Out testResult = new Out(socket);

                switch (testResult.Type)
                {
                    case Out.ResultType.Success:
                        score += testCase.Score;
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestSuccess,testResult.Time ,testResult.Memory));
                        break;
                    case Out.ResultType.WrongAnswer:
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestWA,testResult.Message));
                        break;
                    case Out.ResultType.TimeLimitExceeded:
                        sb.AppendLine(Resources.Moo.MooTester_TestTLE);
                        break;
                    case Out.ResultType.RuntimeError:
                        sb.AppendLine(Resources.Moo.MooTester_TestRE);
                        break;
                    case Out.ResultType.MemoryLimitExceeded:
                        sb.AppendLine(Resources.Moo.MooTester_TestMLE);
                        break;
                    default:
                        sb.AppendLine(Resources.Moo.MooTester_TestUndefinedError);
                        break;
                }
            }

            File.Delete(execFilePath);
            return new TestResult()
            {
                Score = score,
                Info = sb.ToString()
            };
        }

        TestResult InnerTestSpecialJudged(Socket socket, string source, string language, IEnumerable<SpecialJudgedTestCase> cases)
        {
            string execFilePath;
            TestResult compileResult = compile(socket, source, language, out execFilePath);
            if (compileResult != null) return compileResult;

            int score = 0;
            StringBuilder sb = new StringBuilder(Resources.Moo.MooTester_CompilerSuccess).AppendLine().AppendLine();
            foreach (SpecialJudgedTestCase testCase in cases)
            {
                sb.AppendFormat(Resources.Moo.MooTester_TestCaseX, testCase.ID);

                socket.Send(new Message()
                {
                    Type = Message.MessageType.Test,
                    Content = new TestIn()
                    {
                        CmpPath = testCase.Judger.Path,
                        ExecPath = execFilePath,
                        Memory = testCase.MemoryLimit,
                        Time = testCase.TimeLimit,
                        Input = testCase.Input,
                        Output = testCase.Answer
                    }
                }.ToBytes());
                Out testResult = new Out(socket);

                switch (testResult.Type)
                {
                    case Out.ResultType.Success:
                        score += testCase.Score;
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestSuccess, testResult.Time, testResult.Memory));
                        break;
                    case Out.ResultType.WrongAnswer:
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestWA, testResult.Message));
                        break;
                    case Out.ResultType.TimeLimitExceeded:
                        sb.AppendLine(Resources.Moo.MooTester_TestTLE);
                        break;
                    case Out.ResultType.RuntimeError:
                        sb.AppendLine(Resources.Moo.MooTester_TestRE);
                        break;
                    case Out.ResultType.MemoryLimitExceeded:
                        sb.AppendLine(Resources.Moo.MooTester_TestMLE);
                        break;
                    default:
                        sb.AppendLine(Resources.Moo.MooTester_TestUndefinedError);
                        break;
                }
            }

            File.Delete(execFilePath);
            return new TestResult()
            {
                Score = score,
                Info = sb.ToString()
            };
        }
    }
}

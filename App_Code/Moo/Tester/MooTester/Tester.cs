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
                Info = "评测机不配合 T_T"
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
                Info = "评测机不配合 T_T"
            };
        }

        TestResult compile(Socket socket, string source, string language, out string execFilePath)
        {
            execFilePath = null;

            string compileCommand;
            switch (language)
            {
                case "cxx":
                    compileCommand = "g++ -o {Execute} {Source} {.cpp}";
                    break;
                case "c":
                    compileCommand = "gcc -o {Execute} {Source} {.c}";
                    break;
                default:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = "不支持的语言"
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
                        Info = "编译器超时"
                    };
                case Out.ResultType.RuntimeError:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = "编译错误，信息如下：\n\n" + compileResult.Message
                    };
                case Out.ResultType.MemoryLimitExceeded:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = "编译器内存超标"
                    };
                default:
                    return new TestResult()
                    {
                        Score = 0,
                        Info = "编译器心情不好.T_T 请您自便。"
                    };
            }
        }

        TestResult InnerTestTranditional(Socket socket, string source, string language, IEnumerable<TranditionalTestCase> cases)
        {
            string execFilePath;
            TestResult compileResult = compile(socket, source, language, out execFilePath);
            if (compileResult != null) return compileResult;

            int score = 0;
            StringBuilder sb = new StringBuilder("编译成功").AppendLine();
            foreach (TranditionalTestCase testCase in cases)
            {
                sb.AppendLine("测试点" + testCase.ID + "：");

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
                        sb.AppendLine("正确 用时" + testResult.Time + "毫秒 最大内存" + testResult.Memory + "字节");
                        break;
                    case Out.ResultType.WrongAnswer:
                        sb.AppendLine("答案错误，信息如下：" + testResult.Message);
                        break;
                    case Out.ResultType.TimeLimitExceeded:
                        sb.AppendLine("程序超时");
                        break;
                    case Out.ResultType.RuntimeError:
                        sb.AppendLine("程序崩溃");
                        break;
                    case Out.ResultType.MemoryLimitExceeded:
                        sb.AppendLine("超出内存限制");
                        break;
                    default:
                        sb.AppendLine("程序心情不好T_T 请您自便。");
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
            StringBuilder sb = new StringBuilder("编译成功").AppendLine();
            foreach (SpecialJudgedTestCase testCase in cases)
            {
                sb.AppendLine("测试点" + testCase.ID + "：");

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
                        sb.AppendLine("正确 用时" + testResult.Time + "毫秒 最大内存" + testResult.Memory + "字节");
                        break;
                    case Out.ResultType.WrongAnswer:
                        sb.AppendLine("答案错误，信息如下：" + testResult.Message);
                        break;
                    case Out.ResultType.TimeLimitExceeded:
                        sb.AppendLine("程序超时");
                        break;
                    case Out.ResultType.RuntimeError:
                        sb.AppendLine("程序崩溃");
                        break;
                    case Out.ResultType.MemoryLimitExceeded:
                        sb.AppendLine("超出内存限制");
                        break;
                    default:
                        sb.AppendLine("程序心情不好T_T 请您自便。");
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

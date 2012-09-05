using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using System.Text.RegularExpressions;
using Moo.Tester;
using Moo.DB;
using Moo.Utility;
namespace Moo.Tester.MooTester
{
    public class Tester : ITester
    {
        delegate TestResult SocketToTestResult(Socket socket);

        public TestResult TestTranditional(string source, string language, IEnumerable<TranditionalTestCase> cases)
        {
            return WithSocket(socket => InnerTestTranditional(socket, source, language, cases));
        }

        TestResult InnerTestTranditional(Socket socket, string source, string language, IEnumerable<TranditionalTestCase> cases)
        {
            string execFile = Compile(socket, source, Command.GetCommand(language, "src2exe"));
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
                        ExecPath = execFile,
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
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestSuccess, testCase.Score, testResult.Time, testResult.Memory, testResult.Message.Replace('\r',' ').Replace('\n',' ')));
                        break;
                    case Out.ResultType.WrongAnswer:
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestWA, 0, testResult.Time, testResult.Memory, testResult.Message.Replace('\r',' ').Replace('\n', ' ')));
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
                    case Out.ResultType.CompareError:
                        sb.AppendLine(Resources.Moo.MooTester_TestCompareError);
                        break;
                    case Out.ResultType.OutputLimitExceeded:
                        sb.AppendLine(Resources.Moo.MooTester_TestOLE);
                        break;
                    default:
                        sb.AppendLine(Resources.Moo.MooTester_TestUndefinedError);
                        break;
                }
            }

            return new TestResult()
            {
                Score = score,
                Info = sb.ToString()
            };
        }

        public TestResult TestSpecialJudged(string source, string language, IEnumerable<SpecialJudgedTestCase> cases)
        {
            return WithSocket(socket => InnerTestSpecialJudged(socket, source, language, cases));
        }

        TestResult InnerTestSpecialJudged(Socket socket, string source, string language, IEnumerable<SpecialJudgedTestCase> cases)
        {
            string execFile = Compile(socket, source, Command.GetCommand(language, "src2exe"));
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
                        ExecPath = execFile,
                        Memory = testCase.MemoryLimit,
                        Time = testCase.TimeLimit,
                        Input = testCase.Input,
                        Output = testCase.Answer
                    }
                }.ToBytes());
                Out testResult = new Out(socket);

                int currentScore;
                switch (testResult.Type)
                {
                    case Out.ResultType.Success:
                        currentScore = GetScore(ref testResult.Message);
                        score += currentScore;
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestSuccess, currentScore, testResult.Time, testResult.Memory, testResult.Message.Replace('\r',' ').Replace('\n', ' ')));
                        break;
                    case Out.ResultType.WrongAnswer:
                        currentScore = GetScore(ref testResult.Message);
                        score += currentScore;
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestWA, currentScore, testResult.Time, testResult.Memory, testResult.Message.Replace('\r',' ').Replace('\n', ' ')));
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
                    case Out.ResultType.CompareError:
                        sb.AppendLine(Resources.Moo.MooTester_TestCompareError);
                        break;
                    case Out.ResultType.OutputLimitExceeded:
                        sb.AppendLine(Resources.Moo.MooTester_TestOLE);
                        break;
                    default:
                        sb.AppendLine(Resources.Moo.MooTester_TestUndefinedError);
                        break;
                }
            }

            return new TestResult()
            {
                Score = score,
                Info = sb.ToString()
            };
        }

        public TestResult TestInteractive(string source, string language, IEnumerable<InteractiveTestCase> cases)
        {
            return WithSocket(socket => InnerTestInteractive(socket, source, language, cases));
        }

        public TestResult InnerTestInteractive(Socket socket, string source, string language, IEnumerable<InteractiveTestCase> cases)
        {
            string objectFile = Compile(socket, source, Command.GetCommand(language, "src2obj"));
            int score = 0;
            StringBuilder sb = new StringBuilder(Resources.Moo.MooTester_CompilerSuccess).AppendLine().AppendLine();
            foreach (InteractiveTestCase testCase in cases)
            {
                sb.AppendFormat(Resources.Moo.MooTester_TestCaseX, testCase.ID);

                string cmd = Command.GetCommand(language, "obj2exe");
                string objects = objectFile + " \"" + testCase.Invoker.Path + "\"";
                cmd = cmd.Replace("{Object}", objects);

                string execFile = Compile(socket, "", cmd);
                socket.Send(new Message()
                {
                    Type = Message.MessageType.Test,
                    Content = new TestIn()
                    {
                        Time = testCase.TimeLimit,
                        Memory = testCase.MemoryLimit,
                        CmpPath = "",
                        ExecPath = execFile,
                        Input = testCase.TestData,
                        Output = new byte[0]
                    }
                }.ToBytes());

                Out result = new Out(socket);
                int currentScore;
                switch (result.Type)
                {
                    case Out.ResultType.Success:
                        currentScore = GetScore(ref result.Message);
                        score += currentScore;
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestSuccess, currentScore, result.Time, result.Memory, result.Message.Replace('\r',' ').Replace('\n', ' ')));
                        break;
                    case Out.ResultType.WrongAnswer:
                        currentScore = GetScore(ref result.Message);
                        score += currentScore;
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestWA, currentScore, result.Time, result.Memory, result.Message.Replace('\r',' ').Replace('\n', ' ')));
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
                    case Out.ResultType.OutputLimitExceeded:
                        sb.AppendLine(Resources.Moo.MooTester_TestOLE);
                        break;
                    default:
                        sb.AppendLine(Resources.Moo.MooTester_TestUndefinedError);
                        break;
                }
            }

            return new TestResult()
            {
                Score = score,
                Info = sb.ToString()
            };
        }

        public TestResult TestAnswerOnly(IDictionary<int, string> answers, IEnumerable<AnswerOnlyTestCase> cases)
        {
            return WithSocket(socket => InnerTestAnswerOnly(socket, answers, cases));
        }

        TestResult InnerTestAnswerOnly(Socket socket, IDictionary<int, string> answers, IEnumerable<AnswerOnlyTestCase> cases)
        {
            int score = 0;
            StringBuilder sb = new StringBuilder();
            foreach (AnswerOnlyTestCase testCase in cases)
            {
                sb.AppendFormat(Resources.Moo.MooTester_TestCaseX, testCase.ID);

                socket.Send(new Message()
                {
                    Type = Message.MessageType.Test,
                    Content = new TestIn()
                    {
                        CmpPath = "",
                        ExecPath = testCase.Judger.Path,
                        Input = MergeAnswerAndTestData(answers.ContainsKey(testCase.ID) ? answers[testCase.ID] : "", testCase.TestData),
                        Memory = long.Parse(Resources.Moo.MooTester_TestAnswerOnlyMemory),
                        Time = long.Parse(Resources.Moo.MooTester_TestAnswerOnlyTime),
                        Output = new byte[0]
                    }
                }.ToBytes());

                Out result = new Out(socket);
                int currentScore;
                switch (result.Type)
                {
                    case Out.ResultType.Success:
                        currentScore = GetScore(ref result.Message);
                        score += currentScore;
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestSuccess, currentScore, result.Time, result.Memory, result.Message.Replace('\r',' ').Replace('\n', ' ')));
                        break;
                    case Out.ResultType.WrongAnswer:
                        currentScore = GetScore(ref result.Message);
                        score += currentScore;
                        sb.AppendLine(string.Format(Resources.Moo.MooTester_TestWA, currentScore, result.Time, result.Memory, result.Message.Replace('\r',' ').Replace('\n', ' ')));
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
                    case Out.ResultType.OutputLimitExceeded:
                        sb.AppendLine(Resources.Moo.MooTester_TestOLE);
                        break;
                    default:
                        sb.AppendLine(Resources.Moo.MooTester_TestUndefinedError);
                        break;
                }
            }
            return new TestResult()
            {
                Score = score,
                Info = sb.ToString()
            };
        }

        byte[] MergeAnswerAndTestData(string answer, byte[] testData)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(mem))
                {
                    byte[] answerBytes = Encoding.Default.GetBytes(answer);
                    writer.Write((uint)answerBytes.LongLength);
                    writer.Write(answerBytes);
                    writer.Write(testData);
                }
                return mem.ToArray();
            }
        }

        TestResult WithSocket(SocketToTestResult func)
        {
            int failureCount = 0;
            while (failureCount < 5)
            {
                try
                {
                    using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                    {
                        socket.Connect(Resources.Moo.MooTester_TesterIP, int.Parse(Resources.Moo.MooTester_TesterPort));
                        //TODO: FIXME ReceiveTimeout
                        socket.SendTimeout = int.Parse(Resources.Moo.MooTester_SocketSendTimeout);
                        using (MemoryStream mem = new MemoryStream())
                        {
                            using (BinaryWriter writer = new BinaryWriter(mem))
                            {
                                writer.Write(34659308463532339L);
                            }
                            socket.Send(mem.ToArray());
                        }
                        return func(socket);
                    }
                }
                catch (SocketException)
                {
                    failureCount++;
                    Thread.Sleep(1000);
                }
                catch (MooTesterException e)
                {
                    return e.Result;
                }
            }
            Logger.Log("Many SocketExceptions In MooTester");
            return new TestResult()
            {
                Score = 0,
                Info = Resources.Moo.MooTester_NetworkError
            };
        }

        int GetScore(ref string message)
        {
            int result = int.Parse(Regex.Match(message, @"{Score:(\d+)}").Groups[1].Value);
            message = Regex.Replace(message, @"{Score:\d+}", "");
            return result;
        }

        string Compile(Socket socket, string source, string command)
        {
            socket.Send(new Message()
            {
                Type = Message.MessageType.Compile,
                Content = new CompileIn()
                {
                    Command = command,
                    Code = source,
                    Memory = long.Parse(Resources.Moo.MooTester_CompileMemory),
                    Time = long.Parse(Resources.Moo.MooTester_CompileTime)
                }
            }.ToBytes());

            Out compileResult = new Out(socket);
            switch (compileResult.Type)
            {
                case Out.ResultType.Success:
                    return compileResult.Message;
                case Out.ResultType.TimeLimitExceeded:
                    throw new MooTesterException()
                    {
                        Result = new TestResult()
                                {
                                    Score = 0,
                                    Info = Resources.Moo.MooTester_CompilerTLE
                                }
                    };
                case Out.ResultType.RuntimeError:
                    throw new MooTesterException()
                    {
                        Result = new TestResult()
                                {
                                    Score = 0,
                                    Info = string.Format(Resources.Moo.MooTester_CompilerRE, compileResult.Message)
                                }
                    };
                case Out.ResultType.MemoryLimitExceeded:
                    throw new MooTesterException()
                    {
                        Result = new TestResult()
                                {
                                    Score = 0,
                                    Info = Resources.Moo.MooTester_CompilerMLE
                                }
                    };
                case Out.ResultType.OutputLimitExceeded:
                    throw new MooTesterException()
                    {
                        Result = new TestResult()
                                {
                                    Score=0,
                                    Info=Resources.Moo.MooTester_CompileOLE
                                }
                    };
                default:
                    throw new MooTesterException()
                    {
                        Result = new TestResult()
                                {
                                    Score = 0,
                                    Info = Resources.Moo.MooTester_CompilerUndefinedError
                                }
                    };
            }
        }
    }
}

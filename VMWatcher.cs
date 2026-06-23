using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StreamVM_RISC_V
{
    public static class VMWatcher
    {
        public static bool _isConnected=false;
        public static async Task ConnectPipesAsync(string outputPipeName, string inputPipeName)
        {
            try
            {
                Outputer._outputPipe = new NamedPipeClientStream(".", outputPipeName, PipeDirection.Out);
                Outputer._inputPipe = new NamedPipeClientStream(".", inputPipeName, PipeDirection.In);

                await Task.WhenAll(
                    Outputer._outputPipe.ConnectAsync(5000),
                    Outputer._inputPipe.ConnectAsync(5000)
                );
                _isConnected = true;

                // 启动输入监听后台任务
                _ = Task.Run(StartInputWatcher);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"管道连接失败。错误：{ex.Message}");
                _isConnected = false;
                Environment.Exit(0);
            }
        }
        private static async Task StartInputWatcher()
        {
            if (Outputer._inputPipe == null) return;
            using var reader = new StreamReader(Outputer._inputPipe, Encoding.UTF8);
            while (_isConnected)
            {
                try
                {
                    string? line = await reader.ReadLineAsync();
                    if (line == null) break;
                    HandleControlCommand(line.Trim());
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine($"输入接收失败：{ex.Message}");
                    _isConnected = false;
                    break;
                }
            }
        }
        public static List<string> Parse(string input)
        {
            var result = new List<string>();
            int i = 0;
            int len = input.Length;

            while (i < len)
            {
                // 跳过空白字符
                while (i < len && char.IsWhiteSpace(input[i])) i++;
                if (i >= len) break;

                char start = input[i];

                if (IsQuote(start))
                {
                    // 引号括起来的参数
                    int end = FindMatchingQuote(input, i, start);
                    if (end == -1)
                    {
                        // 没有匹配的引号，将剩余部分作为一个参数
                        result.Add(input.Substring(i).Trim());
                        break;
                    }
                    // 提取引号内部内容（不包含引号）
                    string content = input.Substring(i + 1, end - i - 1);
                    result.Add(content);
                    i = end + 1;
                }
                else if (IsBracketStart(start))
                {
                    // 括号括起来的参数
                    char close = GetMatchingBracket(start);
                    int end = FindMatchingBracket(input, i, start, close);
                    if (end == -1)
                    {
                        result.Add(input.Substring(i).Trim());
                        break;
                    }
                    // 提取括号内部内容（不包含最外层括号）
                    string content = input.Substring(i + 1, end - i - 1);
                    result.Add(content);
                    i = end + 1;
                }
                else
                {
                    // 普通参数，读取到下一个空白
                    int startIdx = i;
                    while (i < len && !char.IsWhiteSpace(input[i])) i++;
                    string content = input.Substring(startIdx, i - startIdx);
                    result.Add(content);
                }
            }

            return result;
        }

        private static bool IsQuote(char c) => c == '\'' || c == '\"';

        private static bool IsBracketStart(char c) => c == '(' || c == '[' || c == '{';

        private static char GetMatchingBracket(char open)
        {
            return open switch
            {
                '(' => ')',
                '[' => ']',
                '{' => '}',
                _ => '\0'
            };
        }

        private static int FindMatchingQuote(string s, int start, char quote)
        {
            int i = start + 1;
            while (i < s.Length)
            {
                if (s[i] == '\\' && i + 1 < s.Length)
                {
                    i += 2; // 跳过转义字符
                    continue;
                }
                if (s[i] == quote)
                    return i;
                i++;
            }
            return -1;
        }

        private static int FindMatchingBracket(string s, int start, char open, char close)
        {
            int count = 1;
            int i = start + 1;
            while (i < s.Length)
            {
                if (s[i] == open)
                    count++;
                else if (s[i] == close)
                {
                    count--;
                    if (count == 0)
                        return i;
                }
                i++;
            }
            return -1;
        }
        private static void HandleControlCommand(string cmd)
        {
            List<string> ARGS = Parse(cmd);
            if (!VMControlCommands.TryGetValue(ARGS[0], out var command))
                    throw new Exception("Virtual machine stop error: Ops! Please try using 'stvm.h' for fine control, okay?");
            command(ARGS.Skip(1).ToArray());
        }
        public static Dictionary<string, Action<string[]>> VMControlCommands =new Dictionary<string, Action<string[]>>();
    }
}

using System;
using System.Text;
using System.IO;

namespace ConsoleStrReadWrite
{
    /// <summary>
    /// Coordinate the reading and writing to and from the console.
    /// </summary>
    public class StrReadWrite
    {
        const int Delay = 1750;
        private readonly StringBuilder _sb = new StringBuilder();

        public void WriteString(string str)
        {
            using (var sw = new StringWriter(_sb))
            {
                Console.WriteLine(str);
                sw.Flush();
            }
        }

        public string ReadString()
        {
            _sb.Clear();
            using (var sr = new StringReader(Console.ReadLine() ?? throw new InvalidOperationException()))
            {
                while (sr.Peek() > -1)
                {
                    _sb.Append(sr.ReadLine());
                }
            }

            return _sb.ToString();
        }

        public void ClearConsole()
        {
            Console.Clear();
        }

        public void PauseForKey()
        {
            Console.ReadKey();
        }

        public void DisplayMessageWithDelay(string str, int delay = Delay)
        {
            Console.WriteLine(str);
            System.Threading.Thread.Sleep(delay);
        }
    }
}

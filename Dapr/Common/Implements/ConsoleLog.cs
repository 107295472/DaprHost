using System;

namespace Common.Implements
{
    internal class ConsoleLog : Interface.ILogger
    {
        /// <summary>
        /// 普通信息
        /// </summary>
        /// <param name="message"></param>
        public void LogInfo(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"{DateTime.Now}|INFO|{message}");
            Console.ForegroundColor = color;
        }
        /// <summary>
        /// 告警信息
        /// </summary>
        /// <param name="message"></param>
        public void LogWarn(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"{DateTime.Now}|WARN|{message}");
            Console.ForegroundColor = color;
        }
        /// <summary>
        /// 异常信息
        /// </summary>
        /// <param name="message"></param>
        public void LogError(string message)
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"{DateTime.Now}|ERROR|{message}");
            Console.ForegroundColor = color;
        }
    }
}

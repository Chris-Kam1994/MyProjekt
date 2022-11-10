using System;


namespace Print
{
    static class Console
    {
        public static void WriteLine(string p, ConsoleColor c, params object?[] arguments)
        {
            System.Console.ForegroundColor = c;
            System.Console.WriteLine(p, arguments);
            System.Console.ForegroundColor = ConsoleColor.White;
        }
    }

}
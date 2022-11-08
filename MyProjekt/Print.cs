using System;


namespace Print
{
    class Con
    {
        public static void print(string p, ConsoleColor c)
        {
            Console.ForegroundColor = c;
            Console.WriteLine(p);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }

}
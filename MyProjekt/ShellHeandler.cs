using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace Shell
{
    public static class Command
    {
        // Executes the commands contained in string arguments
        public static void Heandler(string arguments)
        {
            if (OperatingSystem.IsLinux())
            {
                Console.WriteLine("Linux");
                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "/bin/bash";
                proc.StartInfo.Arguments = "-c \" " + arguments + " \"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = false;
                proc.Start();
                


            }
            if (OperatingSystem.IsWindows())
            {

                Console.WriteLine("Windows");
                Process proc = new System.Diagnostics.Process();
                proc.StartInfo.FileName = "cmd";
                proc.StartInfo.Arguments = "/user:Administrator \"cmd /K " + arguments + "\"";
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = false;
                proc.Start();
                
                


            }

        }

    }
}
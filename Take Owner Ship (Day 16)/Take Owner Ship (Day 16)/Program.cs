using System;
using System.Diagnostics;

namespace TakeOwnerShip
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please drag a folder onto the application or into command line");
                Console.ReadKey();
                return;
            }
            Process proc = new Process();
            proc.StartInfo.Arguments = @"/k takeown /f " + '"' + args[0] + '"' + @" /r /d y && icacls " + '"' + args[0] + '"' + @" /grant *S-1-3-4:F /t /c /l /q";
            proc.StartInfo.FileName = "cmd.exe";
            proc.Start();
            Console.ReadKey();
        }
    }
}

using System;
using System.Linq;
using System.IO;

namespace ComboSpliter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please supply input file");
                return;
            }
            
            StreamReader InputFile = new StreamReader(args[0]);
            StreamWriter OutputFile1 = new StreamWriter(@"Output1.txt");
            StreamWriter OutputFile2 = new StreamWriter(@"Output2.txt");

            string CurLine;

            while ((CurLine = InputFile.ReadLine()) != null)
            {
                string[] b = { "a", "a" };
                if (CurLine.Contains(':'))
                {
                    b = CurLine.Split(':');
                }
                else if (CurLine.Contains(';'))
                {
                    b = CurLine.Split(';');
                }
                try
                {
                    OutputFile1.WriteLine(b[1]);
                    OutputFile2.WriteLine(b[0]);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(CurLine);
                    Console.WriteLine(ex);
                }
            }
            OutputFile1.Close();
            OutputFile2.Close();
            InputFile.Close();

            Console.WriteLine("Done Splitting!");
            Console.ReadKey();
        }
    }
}

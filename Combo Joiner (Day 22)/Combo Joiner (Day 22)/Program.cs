using System;
using System.IO;

namespace ComboJoiner
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("Please supply INPUT1 INPUT2");
                return;
            }

            string[] input1 = File.ReadAllLines(args[0]);
            string[] input2 = File.ReadAllLines(args[1]);

            if (input1.Length == input2.Length)
            {
                for (int i = 0; i < input1.Length; i++)
                {
                    File.AppendAllText("output.txt", input1[i] + ":" + input2[i] + "\n");
                }
            }
            else
            {
                Console.WriteLine("make sure input 1 and 2 are the same length");
            }
            Console.WriteLine("DONE!");
            Console.ReadKey();

        }
    }
}
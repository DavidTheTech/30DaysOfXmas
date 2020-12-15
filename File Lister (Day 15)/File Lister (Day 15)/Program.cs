using System;
using System.IO;

namespace FileLister
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please drag a folder or supply a folder path via command line");
                Console.ReadKey();
                return;
            }

            string[] Files = Directory.GetFiles(args[0], "*.*", SearchOption.AllDirectories);
            string[] Folders = Directory.GetDirectories(args[0]);

            string[] FullPath = Files[0].Split('\\');

            string PathToRemove = "";

            for (int i = 0; i < FullPath.Length - 1; i++)
            {
                PathToRemove += FullPath[i] + @"\";
            }

            for (int x = 0; x < Files.Length; x++)
            {
                for (int y = 0; y < Folders.Length; y++)
                {
                    if (Files[x].Contains(Folders[y]))
                    {
                        Files[x] = Files[x].Replace(Folders[y], "");
                    }
                }
            }

            Console.WriteLine("Files in : " + PathToRemove);

            foreach(string CurFile in Files)
            {
                Console.WriteLine(CurFile);
            }

            Console.WriteLine("Done!");

            Console.ReadKey();

        }
    }
}

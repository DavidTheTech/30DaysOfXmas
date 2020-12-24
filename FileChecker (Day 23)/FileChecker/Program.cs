using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FileChecker
{
    class Program
    {
        [STAThreadAttribute]
        static void Main(string[] args)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Open File Dialog";
            fd.ShowDialog();

            OpenFileDialog fd2 = new OpenFileDialog();
            fd2.Title = "Open File Dialog";
            fd2.ShowDialog();

            byte[] FirstFile = File.ReadAllBytes(fd.FileName);
            byte[] SecondFile = File.ReadAllBytes(fd2.FileName);

            if (Enumerable.SequenceEqual(FirstFile, SecondFile))
            {
                Console.WriteLine("Files Identical");
            }
            else
            {
                Console.WriteLine("Files Not Identical");
            }
            Console.ReadKey();
        }
    }
}

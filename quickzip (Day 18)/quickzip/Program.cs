using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Ionic.Zip;

namespace quickzip
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please do quickzip foldername");
                return;
            }

            using (ZipFile zip = new ZipFile())
            {
                zip.AddDirectory(args[0]);
                zip.Save(args[0] + ".zip");
            }
        }
    }
}

using System.IO;
using System.Diagnostics;

namespace RunFromEmbedded
{
    class Program
    {
        static void Main(string[] args)
        {
            string Tmp = Path.GetTempFileName();
            File.WriteAllBytes(Tmp, Properties.Resources.HelloWorld);
            Process proc = new Process();
            proc.StartInfo.FileName = Tmp;
            proc.StartInfo.UseShellExecute = false;
            proc.Start();
        }
    }
}

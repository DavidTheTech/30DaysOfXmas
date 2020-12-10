using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace DownloadBuilder
{
    class Program
    {
        static Random rand = new Random();

        static void Main(string[] args)
        {
            Console.WriteLine("input url");
            string url = Console.ReadLine();
            Console.WriteLine("input file name");
            string filename = Console.ReadLine();
            
            ICodeCompiler ic = new CSharpCodeProvider().CreateCompiler();
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.GenerateExecutable = true;
            cp.CompilerOptions = "/target:winexe";
            cp.OutputAssembly = "Downloader" + rand.Next(1, 40) + ".exe";


            string MainCode = @"using System.Net;

namespace Downloader
{
    class Program
    {
        static void Main()
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadFile(" + '"' + url + '"' + ", " +'"' + filename + '"' + @");
            };
    }
}
}";

            CompilerResults results = ic.CompileAssemblyFromSource(cp, MainCode);


            if (results.Errors.HasErrors)
            {
                string errors = "";
                foreach (CompilerError error in results.Errors)
                {
                    errors += string.Format("Error #{0}: {1} {2}\n", error.ErrorNumber, error.ErrorText, error.Line);
                }
                return;
            }
        }
    }
}

using System;
using System.CodeDom.Compiler;
using Microsoft.CSharp;

namespace BackgroundChangerBuilder
{
    class Program
    {
        static Random rand = new Random();
        static void Main(string[] args)
        {
            Console.WriteLine("Enter Url");
            string URL = Console.ReadLine();

            ICodeCompiler ic = new CSharpCodeProvider().CreateCompiler();
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.GenerateExecutable = true;
            cp.CompilerOptions = "/target:winexe";
            cp.OutputAssembly = "BkChg" + rand.Next(1, 40) + ".exe";

            string MainCode = @"using System;
using System.Runtime.InteropServices;
using System.Net;

namespace background_changer
{
    class Program
    {
        [DllImport(" + '"' + "user32.dll" + '"' + @", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiPram, string pvPram, UInt32 fWinIni);
        private static UInt32 SPI_SETDESKWALLPAPER = 20;
        private static UInt32 SPIF_UPDATEINIFILE = 1;

        static void Main(string[] args)
        {
            new WebClient().DownloadFile(" + '"' + URL + '"' + ", " + '"' + "hello" + '"' +@");
            SystemParametersInfo(SPI_SETDESKWALLPAPER, 1, " + '"' + "hello" +'"' + @", SPIF_UPDATEINIFILE);
        }
    }
}
";

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

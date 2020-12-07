using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Drawing;
using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.IO;

namespace CompileCodeInCSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            ICodeCompiler ic = new CSharpCodeProvider().CreateCompiler();
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.GenerateExecutable = true;
            cp.CompilerOptions = "/target:winexe";
            cp.OutputAssembly = "CompileInCsharp.exe";

            Console.WriteLine("HELLO");

            string MainCode = @"using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

namespace HelloWorld
{
    class Program
    {
        static void Main()
        {
            File.AppendAllText(" + '"' + "test.txt" + '"' + @", " + '"' + @"Hello World" + '"' + @");
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

using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.IO.Compression;
using System.Security.Principal;

namespace StubBuilder
{
    class Program
    {
        static Random rand = new Random();
        

        private static bool IsAdministrator()
        {
            WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }
        static void Main()
        {
            int FileLength = 0;

            string tmp = "Tmp" + rand.Next(1, 200);

            ICodeCompiler ic = new CSharpCodeProvider().CreateCompiler();
            CompilerParameters cp = new CompilerParameters();
            cp.ReferencedAssemblies.Add("System.dll");
            cp.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            cp.ReferencedAssemblies.Add("Microsoft.VisualBasic.dll");
            cp.ReferencedAssemblies.Add("System.Core.dll");
            cp.GenerateExecutable = true;
            cp.CompilerOptions = "/target:winexe";
            cp.OutputAssembly = tmp + ".exe";

            

            Console.WriteLine("Enter c# app path");
            string path = Console.ReadLine();

            byte[] RAWDATA = File.ReadAllBytes(path);
            FileLength = RAWDATA.Length;


            string MainCode = @"using System;
using System.IO;
using System.Reflection;
using System.IO.Compression;
using System.Diagnostics;
using System.Windows.Forms;
using System.Security.Principal;

namespace Stub
{
    class Program
    {
        static void Main(string[] args)
        {
            if (!IsAdministrator())
            {
                MessageBox.Show(" + '"' + "Run as admin" + '"' + @");
                return;
        }

        int PackedFileLength = " + FileLength + @";
        string SelfPath = Assembly.GetEntryAssembly().Location;
        byte[] SelfFile = File.ReadAllBytes(Assembly.GetEntryAssembly().Location);
        byte[] PackedFile = new byte[PackedFileLength];
        Array.Copy(SelfFile, SelfFile.Length - PackedFileLength, PackedFile, 0, PackedFileLength);

            byte[] UnPackedFile = PackedFile;

        string Tmp = Path.GetTempFileName();


            File.WriteAllBytes(Tmp + " + '"' + ".exe" + '"' + @", PackedFile);

            Process proc = new Process();

        proc.StartInfo.FileName = Tmp + " + '"' + ".exe" + '"' + @";
            proc.Start();

        }

    private static bool IsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
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

            byte[] Stub = File.ReadAllBytes(tmp + ".exe");
            byte[] DATA = RAWDATA;
            byte[] newArray = new byte[Stub.Length + DATA.Length];
            Array.Copy(Stub, newArray, Stub.Length);
            Array.Copy(DATA, 0, newArray, Stub.Length, DATA.Length);

            File.WriteAllBytes("OUTPUT" + rand.Next(1, 200) + ".exe", newArray);

        }

        static byte[] Compress(byte[] data)
        {
            using (MemoryStream compressedStream = new MemoryStream())
            {
                using (GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(data, 0, data.Length);
                    zipStream.Close();
                    return compressedStream.ToArray();
                }
            }
        }
    }
}

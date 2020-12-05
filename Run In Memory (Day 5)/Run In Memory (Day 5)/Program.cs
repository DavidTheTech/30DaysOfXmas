using System;
using System.Reflection;

namespace RunInMemory
{
    class Program
    {
        static void Main(string[] args)
        {
            Assembly.Load(data.rawData).EntryPoint.Invoke(0, null);
            Console.ReadKey();
        }
    }
}

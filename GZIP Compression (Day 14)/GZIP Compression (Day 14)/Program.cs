using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GZIPCompression
{
    class Program
    {
        static void Main(string[] args)
        {
            byte[] HelloWorld = { 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64, 0x48, 0x65, 0x6C, 0x6C, 0x6F, 0x20, 0x57, 0x6F, 0x72, 0x6C, 0x64 };
            foreach (byte CurByte in HelloWorld)
            {
                Console.Write("0x" + CurByte.ToString("X2") + ", ");
            }
            Console.WriteLine("= " + Encoding.UTF8.GetString(HelloWorld));

            Console.WriteLine("\n\n\n\n");
            byte[] CompressedBytes = Compress(HelloWorld);
            Console.WriteLine("Compressed: ");

            foreach (byte CurByte in CompressedBytes)
            {
                Console.Write("0x" + CurByte.ToString("X2") + ", ");
            }
            Console.WriteLine("\n\n\n\n");


            byte[] DecompressedBytes = Decompress(CompressedBytes);

            Console.WriteLine("Decompressed: ");
            foreach (byte CurByte in DecompressedBytes)
            {
                Console.Write("0x" + CurByte.ToString("X2") + ", ");
            }


            Console.ReadKey();
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

        static byte[] Decompress(byte[] data)
        {
            using (MemoryStream compressedStream = new MemoryStream(data))
            {
                using (GZipStream zipStream = new GZipStream(compressedStream, CompressionMode.Decompress))
                {
                    using (MemoryStream resultStream = new MemoryStream())
                    {
                        zipStream.CopyTo(resultStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }
    }
}

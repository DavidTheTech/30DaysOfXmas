using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;

namespace FileToImage
{
    class Program
    {
        private BitmapHeader BMPHeader;
        private ImgInfoHeader IMGHeader;

        public struct BitmapHeader
        {
            public ushort sig;
            public int size;
            public uint reserved;
            public uint offset;
        }

        public struct ImgInfoHeader
        {
            public int size;
            public int width;
            public int height;
            public int bitPlanes;
            public int compression;
            public int imageSize;
            public int xRes;
            public int yRes;
            public int nColors;
            public int impColors;
        }

        private List<byte> ReturnBMPHeader(string inputfile)
        {
            CreateImageData(inputfile);
            List<byte> list = new List<byte>();
            list.AddRange(BitConverter.GetBytes(BMPHeader.sig));
            list.AddRange(BitConverter.GetBytes(BMPHeader.size));
            list.AddRange(BitConverter.GetBytes(BMPHeader.reserved));
            list.AddRange(BitConverter.GetBytes(BMPHeader.offset));
            list.AddRange(BitConverter.GetBytes(IMGHeader.size));
            list.AddRange(BitConverter.GetBytes(IMGHeader.width));
            list.AddRange(BitConverter.GetBytes(IMGHeader.height));
            list.AddRange(BitConverter.GetBytes(IMGHeader.bitPlanes));
            list.AddRange(BitConverter.GetBytes(IMGHeader.compression));
            list.AddRange(BitConverter.GetBytes(IMGHeader.imageSize));
            list.AddRange(BitConverter.GetBytes(IMGHeader.xRes));
            list.AddRange(BitConverter.GetBytes(IMGHeader.yRes));
            list.AddRange(BitConverter.GetBytes(IMGHeader.nColors));
            list.AddRange(BitConverter.GetBytes(IMGHeader.impColors));
            return list;
        }

        private void CreateImageData(string inputfile)
        {
            int num = (int)new FileInfo(inputfile).Length;
            int num2 = (int)Math.Sqrt((double)num) - 1;
            num *= 3;
            num += 54;
            BMPHeader = new BitmapHeader
            {
                sig = 19778,
                size = num,
                reserved = 0U,
                offset = 54U
            };
            IMGHeader = new ImgInfoHeader
            {
                size = 40,
                width = num2,
                height = num2,
                bitPlanes = 1572865,
                compression = 0,
                imageSize = 0,
                xRes = 0,
                yRes = 0,
                nColors = 0,
                impColors = 0
            };
        }

        void Decrypt(string input, string output)
        {
            using (FileStream fileStream = new FileStream(output, FileMode.Create))
            {
                byte[] array = File.ReadAllBytes(input).Skip(54).ToArray<byte>();
                for (ulong num = 0UL; num < (ulong)((long)array.Length); num += 3UL)
                {
                    fileStream.WriteByte(array[(int)(checked((IntPtr)num))]);
                }
            }
        }

        void Encrypt(string input, string output)
        {
            using (FileStream fileStream = new FileStream(output, FileMode.Create))
            {
                byte[] array = ReturnBMPHeader(input).ToArray();
                fileStream.Write(array, 0, array.Length);
                byte[] array2 = File.ReadAllBytes(input);
                foreach (byte element in array2)
                {
                    byte[] array4 = Enumerable.Repeat<byte>(element, 3).ToArray<byte>();
                    fileStream.Write(array4, 0, array4.Length);
                }
            }
        }

        static void Main(string[] args)
        {
            start:
            Console.Clear();
            Console.WriteLine("1) Encrypt");
            Console.WriteLine("2) Decrypt");
            string input = Console.ReadLine();

            if (input == "1")
            {
                Console.WriteLine("Enter file name");
                string FI = Console.ReadLine();
                Program prog = new Program();

                try
                {
                    prog.Encrypt(FI, FI + ".bmp");
                }
                catch (Exception ex)
                {
                    goto start;
                }
            }
            if (input == "2")
            {
                Console.WriteLine("Enter file name");
                string FI = Console.ReadLine();
                Program prog = new Program();
                try
                {
                    prog.Decrypt(FI, FI + "_decrypted");
                }
                catch (Exception ex)
                {
                    goto start;
                }
            }
            else
            {
                goto start;
            }
        }
    }
}

using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace TRIPDES
{
    class Program
    {
        static string FirstFilePath = "";

        static void Main(string[] args)
        {
            Begin:
            Console.Clear();
            Console.WriteLine("Decrypt(D) or Encrypt(E) Tripple Des");
            string Mode = Console.ReadLine().ToLower();

            if (Mode == "e")
            {
                OFDTHREAD();
                Console.WriteLine("Correct File? (Y/N)");
                Console.WriteLine(FirstFilePath);
                if (YesNo(Console.ReadLine()))
                {
                    Console.WriteLine("Enter Enc Key");
                    string Key = Console.ReadLine();
                    Encrypt(File.ReadAllBytes(FirstFilePath), true, Key, out byte[] EncedArray);
                    File.WriteAllBytes(FirstFilePath + "_ENC", EncedArray);
                    Console.WriteLine("DONE!");
                    Console.ReadKey();
                    goto Begin;
                }
                else
                {
                    goto Begin;
                }
            }
            else if (Mode == "d")
            {
                OFDTHREAD();
                Console.WriteLine("Correct File? (Y/N)");
                Console.WriteLine(FirstFilePath);
                if (YesNo(Console.ReadLine()))
                {
                    Console.WriteLine("Enter Dec Key");
                    string Key = Console.ReadLine();
                    Decrypt(File.ReadAllBytes(FirstFilePath), true, Key, out byte[] DecedArray);
                    File.WriteAllBytes(FirstFilePath + "_DEC", DecedArray);
                    Console.WriteLine("DONE!");
                    Console.ReadKey();
                    goto Begin;
                }
                else
                {
                    goto Begin;
                }
            }
        }

        static bool YesNo(string Option)
        {
            if (Option.ToLower() == "y" || Option.ToLower() == "yes")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        static void OFDTHREAD()
        {
            Console.WriteLine("Please Specify Path");
            Thread FileDialogThread = new Thread((ThreadStart)(() =>
            {
                OpenFileDialog OFD = new OpenFileDialog();
                if (OFD.ShowDialog() == DialogResult.OK)
                {
                    FirstFilePath = OFD.FileName;
                }

            }));
            FileDialogThread.SetApartmentState(ApartmentState.STA);
            FileDialogThread.Start();
            FileDialogThread.Join();
        }

        public static void Encrypt(byte[] ArrayToEnc, bool useHashing, string key, out byte[] EncedArray)
        {
            byte[] keyArray;
            
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;
            ICryptoTransform cTransform = tdes.CreateEncryptor();
            EncedArray = cTransform.TransformFinalBlock(ArrayToEnc, 0, ArrayToEnc.Length);
            tdes.Clear();
        }

        public static void Decrypt(byte[] ArrayToDec, bool useHashing, string key, out byte[] DecedArray)
        {
            byte[] keyArray;

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));

                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;

            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            DecedArray = cTransform.TransformFinalBlock(ArrayToDec, 0, ArrayToDec.Length);
            tdes.Clear();
        }
    }
}

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
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes.
            //We choose ECB(Electronic code Book)
            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)

            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            //transform the specified region of bytes array to resultArray
            EncedArray = cTransform.TransformFinalBlock(ArrayToEnc, 0, ArrayToEnc.Length);
            //Release resources held by TripleDes Encryptor
            tdes.Clear();
            //Return the encrypted data into unreadable string format
        }

        public static void Decrypt(byte[] ArrayToDec, bool useHashing, string key, out byte[] DecedArray)
        {
            byte[] keyArray;

            if (useHashing)
            {
                //if hashing was used get the hash code with regards to your key
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
                //release any resource held by the MD5CryptoServiceProvider

                hashmd5.Clear();
            }
            else
            {
                //if hashing was not implemented get the byte code of the key
                keyArray = UTF8Encoding.UTF8.GetBytes(key);
            }

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            //set the secret key for the tripleDES algorithm
            tdes.Key = keyArray;
            //mode of operation. there are other 4 modes. 
            //We choose ECB(Electronic code Book)

            tdes.Mode = CipherMode.ECB;
            //padding mode(if any extra byte added)
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            DecedArray = cTransform.TransformFinalBlock(ArrayToDec, 0, ArrayToDec.Length);
            //Release resources held by TripleDes Encryptor                
            tdes.Clear();
            //return the Clear decrypted TEXT
        }
    }
}

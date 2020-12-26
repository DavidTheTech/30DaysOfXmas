using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace CheckForUpdate
{
    class Program
    {
        static void Main(string[] args)
        {
            CheckForUpdate();
        }

        static void CheckForUpdate()
        {
            try
            {
                WebClient webClient = new WebClient();
                Stream stream = webClient.OpenRead("http://Localhost/");

                StreamReader streamReader = new StreamReader(stream);
                string WebVersion = streamReader.ReadToEnd().Trim().ToLower();

                if (!WebVersion.Equals(Application.ProductVersion.ToLower().Trim()))
                {
                    ShowUpdate();
                }
            }
            catch (Exception)
            {
            }
        }

        static void ShowUpdate()
        {
            if (MessageBox.Show("Update is available, want to download?", Application.ProductName, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string fileName = "http://PlaceToDownload.com/";
                Process.Start(fileName);
            }
        }
    }
}

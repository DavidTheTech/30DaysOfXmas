using System;
using System.IO;
using System.Net;

namespace quickftp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("quickftp ip username password localfile serverfile");
            FTP ftp = new FTP("ftp://" + args[0], args[1], args[2]);
            //ftp.createDirectory(args[4]);
            ftp.uploadarray(args[4], File.ReadAllBytes(args[3].ToString()));
        }
    }

    internal class FTP
    {
        private string host;
        private string user;
        private string pass;
        private FtpWebRequest ftpRequest;
        private FtpWebResponse ftpResponse;
        private Stream ftpStream;
        private int bufferSize = 2048;

        public FTP(string hostIP, string userName, string password)
        {
            this.host = hostIP;
            this.user = userName;
            this.pass = password;
        }

        public Stream GenerateStreamFromString(string s)
        {
            MemoryStream stream = new MemoryStream();
            StreamWriter writer = new StreamWriter(stream);
            writer.Write(s);
            writer.Flush();
            stream.Position = 0;
            return stream;
        }

        public void uploadStr(string remoteFile, string Str)
        {
            try
            {
                this.ftpRequest = (FtpWebRequest)WebRequest.Create(this.host + "/" + remoteFile);
                this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
                this.ftpRequest.UseBinary = true;
                this.ftpRequest.UsePassive = true;
                this.ftpRequest.KeepAlive = true;
                this.ftpRequest.Method = "STOR";
                this.ftpStream = this.ftpRequest.GetRequestStream();
                Stream fileStream = GenerateStreamFromString(Str);
                byte[] buffer = new byte[this.bufferSize];
                int count = fileStream.Read(buffer, 0, this.bufferSize);
                try
                {
                    while (count != 0)
                    {
                        this.ftpStream.Write(buffer, 0, count);
                        count = fileStream.Read(buffer, 0, this.bufferSize);
                    }
                }
                catch (Exception ex)
                {
                }
                fileStream.Close();
                this.ftpStream.Close();
                this.ftpRequest = null;
            }
            catch (Exception ex)
            {
            }
        }

        public void uploadarray(string remoteFile, byte[] ary)
        {
            try
            {
                this.ftpRequest = (FtpWebRequest)WebRequest.Create(this.host + "/" + remoteFile);
                this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
                this.ftpRequest.UseBinary = true;
                this.ftpRequest.UsePassive = true;
                this.ftpRequest.KeepAlive = true;
                this.ftpRequest.Method = "STOR";
                this.ftpStream = this.ftpRequest.GetRequestStream();
                Stream fileStream = new MemoryStream(ary);
                byte[] buffer = new byte[this.bufferSize];
                int count = fileStream.Read(buffer, 0, this.bufferSize);
                try
                {
                    while (count != 0)
                    {
                        this.ftpStream.Write(buffer, 0, count);
                        count = fileStream.Read(buffer, 0, this.bufferSize);
                    }
                }
                catch (Exception ex)
                {
                }
                fileStream.Close();
                this.ftpStream.Close();
                this.ftpRequest = null;
            }
            catch (Exception ex)
            {
            }
        }

        public void createDirectory(string newDirectory)
        {
            try
            {
                this.ftpRequest = (FtpWebRequest)WebRequest.Create(this.host + "/" + newDirectory);
                this.ftpRequest.Credentials = new NetworkCredential(this.user, this.pass);
                this.ftpRequest.UseBinary = true;
                this.ftpRequest.UsePassive = true;
                this.ftpRequest.KeepAlive = true;
                this.ftpRequest.Method = "MKD";
                this.ftpResponse = (FtpWebResponse)this.ftpRequest.GetResponse();
                this.ftpResponse.Close();
                this.ftpRequest = null;
            }
            catch (Exception ex)
            {
            }
        }
    }
}

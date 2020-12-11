using System.Collections.Specialized;
using System.Net;

namespace PostToPhp
{
    class Program
    {
        static void Main(string[] args)
        {
            string URL = "http://localhost/";

            NameValueCollection PostData = new NameValueCollection();
            PostData["Username"] = "SomeUsername";
            PostData["Password"] = "SomePassword";

            using (WebClient wc = new WebClient())
            {
                byte[] response = wc.UploadValues(URL, PostData);
                
            }
        }
    }
}

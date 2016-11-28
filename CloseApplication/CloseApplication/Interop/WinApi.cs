using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;

namespace CloseApplication.Interop
{
    public class WinApi
    {
        public delegate bool EnumThreadWindowsCallback(IntPtr hWnd, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);

        [DllImport("user32.dll", CharSet = CharSet.Unicode)]
        protected static extern int GetWindowTextLength(IntPtr hWnd);

        [DllImport("user32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lp1, string lp2);

        [DllImport("user32.dll")]
        public static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowThreadProcessId(IntPtr hWnd, out int processId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        public static extern bool EnumThreadWindows(int dwThreadId, EnumThreadWindowsCallback lpfn, IntPtr lParam);

        [DllImport("User32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        static extern long GetClassName(IntPtr hwnd, StringBuilder lpClassName, long nMaxCount);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", EntryPoint = "SetWindowPos")]
        static extern bool SetWindowPos(
                        int hWnd,           // window handle
                        int hWndInsertAfter,    // placement-order handle
                        int X,          // horizontal position
                        int Y,          // vertical position
                        int cx,         // width
                        int cy,         // height
                        uint uFlags);       // window positioning flags

        [DllImport("User32.dll", SetLastError = true)]
        public static extern IntPtr GetParent(IntPtr hWnd);

        [DllImport("user32.dll")]
        static extern IntPtr GetLastActivePopup(IntPtr hWnd);

        [DllImport("kernel32.dll")]
        public static extern uint GetLastError();

        public static bool EnumTheWindows(IntPtr hWnd, IntPtr lParam)
        {
            int hWndProcessId;
            GetWindowThreadProcessId(hWnd, out hWndProcessId);
            if (processId == hWndProcessId)
            {
                int size = GetWindowTextLength(hWnd);
                int size1 = 1000;
                StringBuilder sb1 = new StringBuilder(size1 + 5);
                GetClassName(hWnd, sb1, size1 + 2);

                #region Make the window visible, if it is invisible
                //ShowWindow(hWnd, 0x0010);
                //SetWindowPos(hWnd.ToInt32(), -1, 0, 0, 500, 500, 0x0040);
                //int threadId = GetWindowThreadProcessId(hWnd, out processId);
                #endregion Make the window visible, if it is invisible

                #region Get parent window
                //var parent = GetParent(hWnd);
                //if (parent != IntPtr.Zero)
                //{
                //    int size_p1 = GetWindowTextLength(parent);
                //    StringBuilder sb_p1 = new StringBuilder(size_p1 + 5);
                //    GetClassName(hWnd, sb_p1, size1 + 2);
                //    StringBuilder sb_p = new StringBuilder(size);
                //    GetWindowText(hWnd, sb_p, size);
                //    Console.WriteLine($"{sb_p1.ToString()}, {sb_p.ToString()}");
                //}
                #endregion Get parent window

                StringBuilder sb = new StringBuilder(size);
                GetWindowText(hWnd, sb, size);

                    var sendMessage = SendMessage(hWnd, Win32Msg.WM_CLOSE, 0, 0);
                    var lastWin32Error = Marshal.GetLastWin32Error();
                    //Console.WriteLine(lastWin32Error);
                    Console.WriteLine($"{hWndProcessId}, {sb1.ToString()}, {sb.ToString()}, {lastWin32Error}");
            }
            return true;
        }

        private static int processId = 0;

        public static void CloseApplication(string applicationName)
        {
            //EnumWindows(EnumTheWindows, IntPtr.Zero);
            //return;
            var processesByName = Process.GetProcessesByName(applicationName);
            if (processesByName.Length == 0)
            {
                return;
            }

            var processCount = processesByName.Length;

            for (var processIndex = 0; processIndex < processCount; processIndex++)
            {
                var process = processesByName[processIndex];
                try
                {
                    if (!process.HasExited)
                    {
                        processId = process.Id;
                        var enumWindows = EnumWindows(EnumTheWindows, process.Handle);
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception.Message);
                }
            }
        }
    }
}

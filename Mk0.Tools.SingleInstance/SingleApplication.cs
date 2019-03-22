using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.Threading;
using System.Reflection;
using System.IO;

namespace Mk0.Tools.SingleInstance
{
    /// <summary>
    /// Summary description for SingleApp.
    /// </summary>
    public class SingleApplication
    {
        public SingleApplication()
        {
        }

        /// <summary>
        /// Imports 
        /// </summary>
        [DllImport("user32.dll")]
        private static extern int ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern int SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        private static extern int IsIconic(IntPtr hWnd);

        /// <summary>
        /// GetCurrentInstanceWindowHandle
        /// </summary>
        /// <returns></returns>
        private static IntPtr GetCurrentInstanceWindowHandle()
        {
            IntPtr hWnd = IntPtr.Zero;
            Process process = Process.GetCurrentProcess();
            Process[] processes = Process.GetProcessesByName(process.ProcessName);
            foreach (Process _process in processes)
            {
                if (_process.Id != process.Id &&
                    _process.MainModule.FileName == process.MainModule.FileName &&
                    _process.MainWindowHandle != IntPtr.Zero)
                {
                    hWnd = _process.MainWindowHandle;
                    break;
                }
            }
            return hWnd;
        }
        /// <summary>
        /// SwitchToCurrentInstance
        /// </summary>
        private static void SwitchToCurrentInstance()
        {
            IntPtr hWnd = GetCurrentInstanceWindowHandle();
            if (hWnd != IntPtr.Zero)
            {
                if (IsIconic(hWnd) != 0)
                {
                    ShowWindow(hWnd, SW_RESTORE);
                }

                SetForegroundWindow(hWnd);
            }
        }

        /// <summary>
        /// Execute a form base application if another instance already running on
        /// the system activate previous one
        /// </summary>
        /// <param name="frmMain">main form</param>
        /// <param name="appSetting">your app can set if single instance (true) or multi instance (false)</param>
        /// <returns>true if no previous instance is running</returns>
        public static bool Run(Form frmMain, bool appSetting=true)
        {
            if (IsAlreadyRunning() && appSetting)
            {
                SwitchToCurrentInstance();
                return false;
            }
            Application.Run(frmMain);
            return true;
        }

        /// <summary>
        /// for console base application
        /// </summary>
        /// <returns></returns>
        public static bool Run()
        {
            if (IsAlreadyRunning())
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// check if given exe alread running or not
        /// </summary>
        /// <returns>returns true if already running</returns>
        private static bool IsAlreadyRunning()
        {
            string strLoc = Assembly.GetExecutingAssembly().Location;
            FileSystemInfo fileInfo = new FileInfo(strLoc);
            string sExeName = fileInfo.Name;

            mutex = new Mutex(true, "Global\\" + sExeName, out bool bCreatedNew);
            if (bCreatedNew)
            {
                mutex.ReleaseMutex();
            }

            return !bCreatedNew;
        }

        static Mutex mutex;
        const int SW_RESTORE = 9;
    }
}

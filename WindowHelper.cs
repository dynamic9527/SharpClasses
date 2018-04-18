using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOOL
{
    class WindowHelper
    {
        public static string childClassName = "";
        public static string childWindowName = "";
        public static IntPtr childHwnd = IntPtr.Zero;
        public static IntPtr FindWindow(string partTitle,string className)
        {
            IntPtr hwnd = Win32API.GetDesktopWindow();
            hwnd = Win32API.GetWindow(hwnd, Win32API.GW_CHILD);

            StringBuilder Title = new StringBuilder(512);

            while (hwnd!= IntPtr.Zero)
            {
                if (Win32API.IsWindowVisible(hwnd))
                {
                    if (partTitle != null)
                    {
                        Win32API.GetWindowText(hwnd, Title, Title.Capacity);
                        if (Title.ToString().IndexOf(partTitle) < 0)
                        {
                            hwnd = Win32API.GetWindow(hwnd, Win32API.GW_HWNDNEXT);
                            continue;
                        }
                    }
                    if (className != null)
                    {
                        StringBuilder sb = new StringBuilder(255);
                        Win32API.GetClassName(hwnd, sb, 255);
                        if (sb.ToString() != className)
                        {
                            hwnd = Win32API.GetWindow(hwnd, Win32API.GW_HWNDNEXT);
                            continue;
                        }
                    }
                    return hwnd;
                }
                hwnd = Win32API.GetWindow(hwnd, Win32API.GW_HWNDNEXT);
            }

            return IntPtr.Zero;
        }
        public static IntPtr GetChildWindow(IntPtr hwndParent,string child_windowName,string child_className)
        {
            childHwnd = IntPtr.Zero;
            childWindowName = child_windowName;
            childClassName = child_className;
            Win32API.EnumChildWindows(hwndParent, new Win32API.EnumWindowsProc(EnumWindowCallBack), IntPtr.Zero);
            return childHwnd;
        }
        /// <summary>
        /// 回调函数，枚举所有子窗口
        /// </summary>
        /// <param name="hwnd"></param>
        /// <param name="lParam"></param>
        /// <returns>返回true，继续枚举；返回false，结束枚举</returns>
        private static bool EnumWindowCallBack(IntPtr hwnd, IntPtr lParam)
        {
            StringBuilder Title = new StringBuilder(255);
            StringBuilder className = new StringBuilder(255);
            Win32API.GetWindowText(hwnd, Title, Title.Capacity);
            Win32API.GetClassName(hwnd, className, 255);
            if (childWindowName != null)
            {
                if (childWindowName != Title.ToString())
                    return true;
            }
            if (childClassName != null)
            {
                if (className.ToString() != childClassName)
                    return true;
            }
            childHwnd = hwnd;
            return false;
        }
    }
}

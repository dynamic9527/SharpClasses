using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Runtime.InteropServices;
//using WinAPI.Enums;
//using WinAPI.Struct;
//using WinAPI.Input;

namespace TOOL
{
    public class Win32API
    {
        #region User32.dll 常量
        public const int HWND_TOPMOST = -1;
        public const int SWP_SHOWWINDOW = 0x0040;
        public const int SWP_NOSIZE = 0x0001;

        public const int GW_HWNDFIRST = 0;
        public const int GW_HWNDLAST = 1;
        public const int GW_HWNDNEXT = 2;
        public const int GW_HWNDPREV = 3;
        public const int GW_OWNER = 4;
        public const int GW_CHILD = 5;

        public const int VK_RETURN = 0x0D;
        public const int VK_LEFT = 0x25;
        public const int VK_UP = 0x26;
        public const int VK_RIGHT = 0x27;
        public const int VK_DOWN = 0x28;

        public const int WM_IME_CHAR = 0x0286;
        public const int WM_KEYDOWN = 0x0100;
        public const int WM_KEYUP = 0x0101;
        public const int WM_CHAR = 0x0102;
        public const int WM_SETFOCUS = 0x0007;
        public const int WM_KILLFOCUS = 0x0008;
        public const int WM_ENABLE = 0x000A;
        public const int WM_SETREDRAW = 0x000B;
        public const int WM_SETTEXT = 0x000C;
        public const int WM_GETTEXT = 0x000D;
        public const int WM_GETTEXTLENGTH = 0x000E;
        public const int WM_PAINT = 0x000F;
        public const int WM_CLOSE = 0x0010;
        public const int BM_CLICK = 0xF5;

        public const int MOUSEEVENTF_LEFTDOWN = 0x0002;//模拟鼠标移动
        public const int MOUSEEVENTF_MOVE = 0x0001;//模拟鼠标左键按下
        public const int MOUSEEVENTF_LEFTUP = 0x0004;//模拟鼠标左键抬起
        public const int MOUSEEVENTF_ABSOLUTE = 0x8000;//鼠标绝对位置
        public const int MOUSEEVENTF_RIGHTDOWN = 0x0008; //模拟鼠标右键按下 
        public const int MOUSEEVENTF_RIGHTUP = 0x0010; //模拟鼠标右键抬起 
        public const int MOUSEEVENTF_MIDDLEDOWN = 0x0020; //模拟鼠标中键按下 
        public const int MOUSEEVENTF_MIDDLEUP = 0x0040;// 模拟鼠标中键抬起 

        #endregion

        #region 委托
        [UnmanagedFunctionPointerAttribute(CallingConvention.StdCall)]
        public delegate int HOOKPROC(int code, IntPtr wParam, IntPtr lParam);
        public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);
        #endregion

        #region 结构体
        public struct RECT
        {
            public int left;
            public int top;
            public int right;
            public int bottom;

            public RECT(int left, int top, int right, int bottom)
            {
                this.left = left;
                this.top = top;
                this.right = right;
                this.bottom = bottom;
            }
            /// <summary>
            /// 转换到Rectangle类.
            /// </summary>
            /// <returns></returns>
            //public Rectangle ToRectangle()
            //{
            //    return new Rectangle(left, top, right - left, bottom - top);
            //}
            public int Width
            {
                get { return right - left; }
            }

            public int Height
            {
                get { return bottom - top; }
            }

        }
        #endregion

        #region User32.dll 函数
        /// <summary>
        /// 获得窗口句柄
        /// </summary>
        /// <param name="className">窗体类名</param>
        /// <param name="windowName">窗口标题</param>
        /// <returns>句柄</returns>
        [DllImport("User32.dll")]
        public static extern IntPtr FindWindow(string className, string windowName);
        /// <summary>
        /// 在窗口列表中寻找与指定条件相符的第一个子窗口.
        /// </summary>
        /// <param name="hwndParent">要查找的子窗口所在的父窗口的句柄.如果设置了hwndParent则表示从这个hwndParent指向的父窗口中搜索子窗口.</param>
        /// <param name="hwndChildAfter">子窗口句柄.查找从在Z序中的下一个子窗口开始.子窗口必须为hwndParent窗口的直接子窗口而非后代窗口.如果HwndChildAfter为NULL,查找从hwndParent的第一个子窗口开始.如果hwndParent 和 hwndChildAfter同时为NULL,则函数查找所有的顶层窗口及消息窗口.</param>
        /// <param name="lpszClass">指向一个指定了类名的空结束字符串,或一个标识类名字符串的成员的指针.如果该参数为一个成员,则它必须为前次调用theGlobaIAddAtom函数产生的全局成员.该成员为16位,必须位于lpClassName的低16位,高位必须为0.</param>
        /// <param name="lpszWindow">指向一个指定了窗口名(窗口标题)的空结束字符串.如果该参数为null,则为所有窗口全匹配.</param>
        /// <returns>窗口的句柄</returns>
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern IntPtr GetDesktopWindow();

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindow(IntPtr hwnd, int wCmd);

        [DllImport("user32.dll")]
        public static extern bool IsWindowVisible(IntPtr hwnd);

        [DllImport("User32.dll")]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, char wParam, int lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, StringBuilder lParam);

        [DllImport("user32.dll")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, string lParam);

        [DllImport("user32.dll")]
        public static extern int PostMessage(IntPtr hwnd, int wMsg, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern bool EnumChildWindows(IntPtr hwndParent, EnumWindowsProc lpEnumFunc, IntPtr lParam);

        [DllImport("user32.dll")]
        public static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll")]
        public static extern IntPtr SetFocus(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern int SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int x, int y, int Width, int Height, int flags);

        [DllImport("user32.dll")]
        public static extern bool GetWindowRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        public extern static int GetClientRect(IntPtr hWnd, out RECT rect);

        [DllImport("user32.dll")]
        public static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        public static extern int SetCursorPos(int x, int y);

        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        #endregion

        #region gdi32
        [DllImport("gdi32.dll")]
        public static extern bool BitBlt(IntPtr hObject, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hObjectSource, int nXSrc, int nYSrc, int dwRop);
        #endregion

        #region Dbghelp
        [DllImportAttribute("Dbghelp.dll", EntryPoint = "MakeSureDirectoryPathExists")]
        public static extern bool MakeSureDirectoryPathExists(string DirPath);
        #endregion
    }

}

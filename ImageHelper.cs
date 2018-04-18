using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TOOL
{
    class ImageHelper
    {
       
        private static IntPtr hDesk = Win32API.GetDesktopWindow();
        private static Win32API.RECT rt = GetRect(hDesk);
        public static byte[] pData = new byte[1920 * 1080 * 3];
        public static byte[] tData = new byte[512 * 512 * 3];
        public static bool GetDcData()
        {
            Bitmap dstBit = GetScreenCapture(hDesk,rt);
            //dstBit.Save(@"E:\me.bmp");
            Rectangle dstRectN = new Rectangle(0, 0, dstBit.Width, dstBit.Height);
            BitmapData pBData = dstBit.LockBits(dstRectN, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            //if (pBData.Stride * pBData.Height > rt.Height * rt.Width * 3)
            //{
            //    //FileRW.WriteToFile("获取频幕数据出错！");
            //    dstBit.UnlockBits(pBData);
            //    dstBit.Dispose();
            //    return false;
            //}
            System.Runtime.InteropServices.Marshal.Copy(pBData.Scan0, pData, 0, pBData.Stride * pBData.Height);
            dstBit.UnlockBits(pBData);
            dstBit.Dispose();
            return true;
        }
        public static Bitmap GetScreenCapture(IntPtr hWnd, Win32API.RECT dstRect)
        {
            //获得当前屏幕的大小
            //int width = Screen.PrimaryScreen.Bounds.Width;
            //int height = Screen.PrimaryScreen.Bounds.Height;
            const int SRCCOPY = 0x00CC0020;
            const int CAPTUREBLT = 0x40000000;
            Win32API.RECT rect;
            if (hWnd == IntPtr.Zero)
                hWnd = Win32API.GetDesktopWindow();
            Win32API.GetClientRect(hWnd, out rect);
            //if (rect.Width < dstRect.Width || rect.Height < dstRect.Height)
            //{
            //    AppLog.WriteError(string.Format("截图目标窗口句柄大小【{0},{1}】",rect.Width,rect.Height));
            //    AppLog.WriteError(string.Format("截图矩形设置大小【{0},{1}】", dstRect.Width, dstRect.Height));
            //    AppLog.WriteError("截图矩形设置错误");
            //    return null;
            //}
            //int width = dstRect.right - dstRect.left;
            //int height = dstRect.bottom - dstRect.top;

            Graphics grpScreen = Graphics.FromHwnd(hWnd);
            //创建一个以当前屏幕为模板的图象
            Image MyImage = new Bitmap(dstRect.Width, dstRect.Height, grpScreen);
            //创建以屏幕大小为标准的位图 
            Graphics g2 = Graphics.FromImage(MyImage);
            //得到屏幕的DC
            IntPtr dc1 = grpScreen.GetHdc();
            //得到Bitmap的DC                      
            IntPtr dc2 = g2.GetHdc();
            Win32API.BitBlt(dc2, 0, 0, dstRect.Width, dstRect.Height, dc1, dstRect.left, dstRect.top, SRCCOPY | CAPTUREBLT);
            grpScreen.ReleaseHdc(dc1);
            g2.ReleaseHdc(dc2);
            grpScreen.Dispose();
            g2.Dispose();
            return (Bitmap)MyImage;
        }
        private static Win32API.RECT GetRect(IntPtr hDesk)
        {
            Win32API.RECT rect = new Win32API.RECT();
            Win32API.GetWindowRect(hDesk, out rect);
            return rect;

        }
        public static bool GetPicData(string filePic, ref int w, ref int h, ref byte[] data)
        {
            if (!File.Exists(filePic))
            {
                w = 0;
                h = 0;
                return false;
            }
            Bitmap srcBit = (Bitmap)Bitmap.FromFile(filePic, false);
            Rectangle srcRect = new Rectangle(0, 0, srcBit.Width, srcBit.Height);
            BitmapData pBData = srcBit.LockBits(srcRect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            w = pBData.Stride;
            h = srcBit.Height;
            System.Runtime.InteropServices.Marshal.Copy(pBData.Scan0, data, 0, pBData.Stride * pBData.Height);
            srcBit.UnlockBits(pBData);
            srcBit.Dispose();
            return true;
        }
        public static Point fPicBtoB(byte[] ttData, int w, int h, int x1, int y1, int x2, int y2, int xiangshi, bool bFirst, int mohu)
        {
            Point PT = new Point(-1, -2);
            //bool fFalse;
            byte r, g, b, now_r, now_g, now_b;

            int wx = rt.Width * 3;
            int wy = rt.Height;

            if (x1 < 0)
                x1 = 0;
            if (y1 < 0)
                y1 = 0;
            //循环比较
            if (w > wx || h > wy || x1 < 0 || y1 < 0)
                return PT;
            if ((x2 == 0) || (x2 > rt.Width))
                x2 = rt.Width;
            if ((y2 == 0) || (y2 > rt.Height))
                y2 = rt.Height;


            //循环比较
            for (int y = y1; y < y2 - h - 1; y++)
            {
                for (int x = x1; x < x2 - w / 3 - 1; x++)
                {

                    int nFalse = 0;
                    for (int i = 0; i < h - 1; i++)
                    {
                        if (nFalse > (w / 3 * h * mohu / 100))
                            break;
                        for (int j = 0; j < w / 3 - 1; j++)
                        {
                            if (nFalse > (w / 3 * h * mohu / 100))
                                break;

                            b = ttData[i * w + j * 3];
                            g = ttData[i * w + j * 3 + 1];
                            r = ttData[i * w + j * 3 + 2];
                            now_r = pData[wx * (y + i) + (x + j) * 3 + 2];
                            now_g = pData[wx * (y + i) + (x + j) * 3 + 1];
                            now_b = pData[wx * (y + i) + (x + j) * 3];

                            if (bFirst)
                            {
                                if (((Math.Abs(now_r - r) < xiangshi) && (Math.Abs(now_g - g) < xiangshi) && (Math.Abs(now_b - b) < xiangshi)) || ((b == tData[0]) && (g == tData[1]) && (r == tData[2])))
                                { }
                                else
                                    nFalse++;
                            }
                            else
                            {
                                if (((Math.Abs(now_r - r) < xiangshi) && (Math.Abs(now_g - g) < xiangshi) && (Math.Abs(now_b - b) < xiangshi)))
                                { }
                                else
                                    nFalse++;
                            }
                        }

                    }
                    if (nFalse <= (w / 3 * h * mohu / 100))
                    {
                        PT.X = x;
                        PT.Y = y;
                        return PT;
                    }
                }
            }
            return PT;
        }
        public static Point fPic(IntPtr hwnd,string pic,int left=0,int top=0,int right=0,int bottom=0,int xiangshi=40,int mohu=100)
        {
            if (Win32API.IsWindowVisible(hwnd))
            {
                Win32API.RECT rect = GetRect(hwnd);
                left += rect.left;
                top += rect.top;
                if (right > 0)
                    right += rect.left;
                else
                    right = rect.right;
                if (bottom > 0)
                    bottom += rect.top;
                else
                    bottom = rect.bottom;
            }
            mohu = 100 - mohu;
            Point PT = new Point(-1, -2);
            if (!GetDcData())
                return PT;
           
            int w2 = 0, h2 = 0;
            if (!GetPicData(pic, ref w2, ref h2, ref tData))
                return PT;
            return fPicBtoB(tData, w2, h2, left, top, right, bottom, xiangshi, true, mohu);
        }

    }
}

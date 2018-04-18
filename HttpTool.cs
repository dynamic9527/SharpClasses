using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;

namespace TOOL
{
    class HttpTool
    {
        private const int INTERNET_COOKIE_HTTPONLY = 0x00002000;
        [DllImport("wininet.dll", SetLastError = true)]
        private static extern bool InternetGetCookieEx(
            string url,
            string cookieName,
            StringBuilder cookieData,
            ref int size,
            int flags,
            IntPtr pReserved);
        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool InternetSetCookie(
            string lpszUrlName, string lbszCookieName, string lpszCookieData);

        [DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int InternetSetCookieEx(
            string lpszURL, string lpszCookieName, string lpszCookieData, int dwFlags, IntPtr dwReserved);

        public static string HttpGet(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                //request.ContentType = "text/html;charset=UTF-8";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "application/json";

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream myResponseStream = response.GetResponseStream();
                StreamReader myStreamReader = new StreamReader(myResponseStream, Encoding.GetEncoding("UTF-8"));
                string retString = myStreamReader.ReadToEnd();
                myStreamReader.Close();
                myResponseStream.Close();
                return retString;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        public static string PostUrlData(string url, string postData)
        {
            //string postData = "user=123&pass=456"; // 要发放的数据 
            try
            {
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                //待请求参数数组字符串

                System.Net.HttpWebRequest objWebRequest = (System.Net.HttpWebRequest)System.Net.WebRequest.Create(url);
                objWebRequest.Method = "POST";
                objWebRequest.ContentType = "application/x-www-form-urlencoded";
                objWebRequest.ContentLength = byteArray.Length;
                Stream newStream = objWebRequest.GetRequestStream();
                // Send the data. 
                newStream.Write(byteArray, 0, byteArray.Length); //写入参数 
                newStream.Close();

                System.Net.HttpWebResponse response = (System.Net.HttpWebResponse)objWebRequest.GetResponse();
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);//Encoding.Default
                string textResponse = sr.ReadToEnd(); // 返回的数据
                return textResponse;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// Http下载文件
        /// </summary>
        public static bool HttpDownloadFile(string url, string path)
        {
            try
            {
                // 设置参数
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                //发送请求并获取相应回应数据
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                //直到request.GetResponse()程序才开始向目标网页发送Post请求
                Stream responseStream = response.GetResponseStream();
                //创建本地文件写入流
                Stream stream = new FileStream(path, FileMode.Create);
                byte[] bArr = new byte[1024];
                int size = responseStream.Read(bArr, 0, (int)bArr.Length);
                while (size > 0)
                {
                    stream.Write(bArr, 0, size);
                    size = responseStream.Read(bArr, 0, (int)bArr.Length);
                }
                stream.Close();
                responseStream.Close();
                return true;
            }
            catch (Exception e)
            {
                TOOL.LogTool.WriteLog(e.Message);
            }
            return false;
        }
        /// <summary>
        /// 获取cookie(包括sessionID)
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string GetCookieString(string url)
        {
            int size = 512;
            StringBuilder sb = new StringBuilder(size);
            if (!InternetGetCookieEx(url, null, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero))
            {
                if (size < 0)
                {
                    return null;
                }
                sb = new StringBuilder(size);
                if (!InternetGetCookieEx(url, null, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero))
                {
                    return null;
                }
            }
            return sb.ToString();
        }
    }
}

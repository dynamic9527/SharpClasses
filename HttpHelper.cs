using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
//using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace BigBird
{
    class HttpHelper
    {
        public static CookieContainer myCookieContainer = new CookieContainer();
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
        public static string RequestUrl(string url, CookieContainer myCookieContainer, string strRefer, string host)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    Uri uri = new Uri(url);
                    HttpWebRequest myReq = (HttpWebRequest)WebRequest.Create(url);
                    myReq.UserAgent = "User-Agent:Mozilla/4.0 (compatible; MSIE 6.0; Windows NT 5.2; .NET CLR 1.0.3705";
                    //myReq.Accept = "*/*";
                    myReq.Method = "GET";
                    //myReq.Host = host;
                    myReq.ContentType = "application/json";
                    //myReq.KeepAlive = true;
                    myReq.Timeout = 1000 * 60;
                    //myReq.Headers.Add("Accept-Language", "zh-cn,en-us;q=0.5");
                    myReq.CookieContainer = myCookieContainer;
                    //myReq.Referer = strRefer;
                    //ServicePointManager.DefaultConnectionLimit = 50;
                    HttpWebResponse result = (HttpWebResponse)myReq.GetResponse();
                    Stream receviceStream = result.GetResponseStream();
                    StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.UTF8);
                    string strHTML = readerOfStream.ReadToEnd();
                    readerOfStream.Close();
                    receviceStream.Close();
                    if (result != null)
                        result.Close();
                    if (myReq != null)
                        myReq.Abort();
                    return strHTML;
                }
                catch (WebException we)
                {
                    TOOL.LogTool.WriteLog(we.Message);
                    continue;
                }
            }
            return "";
        }
        public static string HttpGet(string Url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
                request.Method = "GET";
                request.CookieContainer = myCookieContainer;
                //request.ContentType = "text/html;charset=UTF-8";
                request.ContentType = "application/x-www-form-urlencoded";
                //request.ContentType = "application/json";
                //request.Host = "www.taobao.com";

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
    }
}

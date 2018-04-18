using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace TOOL
{
    class StringTool
    {
        public static string GetCut(string strSource)
        {
            char[] anychar = new char[1];
            anychar[0] = '\0';
            int len = strSource.IndexOfAny(anychar);
            if (len > 0)
            {
                return strSource.Substring(0, len);
            }
            return strSource;
        }
        public static string ReverseFindStr(string strSource, string strKeyBegin, string strKeyEnd,int Start)
        {
            if (Start == 0)
                Start = strSource.Length;
            int left = Start;
            int begin = -1;
            while ((left=left -strKeyBegin.Length) >= 0)
            {
                begin = strSource.Substring(left, Start - left).IndexOf(strKeyBegin);
                if (begin >= 0)
                {
                    begin = left + begin + strKeyBegin.Length;
                    break;
                }
            }
            if (begin < 0)
                return "";
            int end = strSource.IndexOf(strKeyEnd, begin);
            if (end < 0)
                return "";
            return strSource.Substring(begin, end - begin);
        }
        public static string FindStr(string strSource, string strKeyBegin, string strKeyEnd)
        {
            int begin = strSource.IndexOf(strKeyBegin, 0);
            if (begin < 0)
                return "";
            begin += strKeyBegin.Length;
            int end = strSource.IndexOf(strKeyEnd, begin);
            if (end < 0)
            {
                if (strKeyEnd == "\r\n")
                    end = strSource.Length;
                else
                    return "";
            }
            return strSource.Substring(begin, end - begin);
        }
        public static string FindStr(string strSource, string strKeyBegin, string strKeyEnd, int Start)
        {
            int begin = strSource.IndexOf(strKeyBegin, Start);
            if (begin < 0)
                return "";
            begin += strKeyBegin.Length;
            int end = strSource.IndexOf(strKeyEnd, begin);
            if (end < 0)
                return "";
            return strSource.Substring(begin, end - begin);
        }
        public static string FindStr(string strSource, string strKeyBegin, string strKeyEnd, int Start, int count)
        {
            if (count == 0)
                count = strSource.Length;
            int begin = strSource.IndexOf(strKeyBegin, Start, count);
            if (begin < 0)
                return "";
            begin += strKeyBegin.Length;
            int end = strSource.IndexOf(strKeyEnd, begin);
            if (end < 0)
                return "";
            return strSource.Substring(begin, end - begin);
        }
    }
}

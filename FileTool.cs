using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace TOOL
{
    /// <summary>
    /// 文件操作类
    /// </summary>
    class FileTool
    {
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section,string key, string def, 
            StringBuilder retVal,int size, string filePath);
        [DllImport("kernel32")]
        public static extern long WritePrivateProfileString(string section,string key, string val, string filePath);
        /// <summary>
        /// 读取ini文件
        /// </summary>
        /// <param name="section"></param>
        /// <param name="key"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static string ReadINI(string section,string key,string filePath)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(section, key, "", temp, 1024, filePath);
			return temp.ToString();
        }
    }
}

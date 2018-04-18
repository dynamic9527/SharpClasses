using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace TOOL
{
    class MD5Tool
    {
        /// <summary>
        /// 获取文字内容的MD5值
        /// </summary>
        /// <param name="PathFile"></param>
        /// <returns></returns>
        public static string GetMD5HashFromText(string PathFile)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.UTF8.GetBytes(PathFile);
            byte[] md5data = md5.ComputeHash(data);
            md5.Clear();
            string str = "";
            for (int i = 0; i < md5data.Length; i++)
            {
                str += md5data[i].ToString("X").PadLeft(2, '0');
            }
            return str;
        }
        /// <summary>
        /// 获取文件的MD5值
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static string GetMD5HashFromFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                TOOL.LogTool.WriteLog(fileName + "不存在");
                return "";
            }
            try
            {
                FileStream file = new FileStream(fileName, FileMode.Open);
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                TOOL.LogTool.WriteLog("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
            return "";
        }
    }
}

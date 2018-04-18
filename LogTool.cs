using System;
using System.Collections.Generic;
using System.Text;
using System.IO;


namespace TOOL
{
    /// <summary>
    /// 日志操作类
    /// 作者:邵建
    /// 日期:2017-04-26
    /// </summary>
    class LogTool
    {
        public static string logPath = AppDomain.CurrentDomain.BaseDirectory + "Log\\";

        static LogTool()
        {
            if (!Directory.Exists(logPath))
                Directory.CreateDirectory(logPath);
        }

        /// <summary>
        /// 获取日志文件完整名称
        /// </summary>
        //public static string LogPathName = logPath + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + ".log";


        /// <summary>
        /// 将字符串写入日志文件中
        /// </summary>
        /// <param name="strLog"></param>
        public static void WriteLog(string strLog)
        {
            string LogPathName = logPath + DateTime.Now.ToString("yyyy-MM-dd") + ".log";
            using (StreamWriter sw = new StreamWriter(LogPathName, true, Encoding.Default))
            {
                sw.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strLog);
            }        
        }
        public static void WriteLog(string fileName,string strLog)
        {
            string LogPathName = logPath + fileName + ".log";
            using (StreamWriter sw = new StreamWriter(LogPathName, true, Encoding.Default))
            {
                sw.WriteLine("{0} {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"), strLog);
            }
        }
        public static void WriteFile(string strFile,string strLog)
        {   
            using (StreamWriter sw = new StreamWriter(strFile, false, Encoding.Default))
            {
                sw.WriteLine("{0}", strLog);
            }
        }
        /// <summary>
        /// 删除N天前的日志文件
        /// </summary>
        /// <param name="Days"></param>
        public static void DeleteLogFile(int Days)
        {
            DirectoryInfo dir = new DirectoryInfo(logPath);
            foreach (FileInfo fi in dir.GetFiles())
            {
                if (fi.CreationTime < DateTime.Today.AddDays(0 - Days))
                    fi.Delete();
            }
        }


    }
}

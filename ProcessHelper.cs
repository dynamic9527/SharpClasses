using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace TOOL
{
    class ProcessTool
    {
        /// <summary>
        /// 根据进程号关闭进程
        /// </summary>
        /// <param name="pid"></param>
        public static void KillProcess(int pid)
        {
            if (pid == 0)
                return;
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (pid == item.Id)
                {
                    item.Kill();
                    return;
                }
            }
        }
        /// <summary>
        /// 根据进程名关闭进程
        /// </summary>
        /// <param name="processName"></param>
        public static void KillProcess(string processName)
        {
            try
            {
                Process[] ps = Process.GetProcesses();
                if (processName.IndexOf(".") > 0)
                    processName = processName.Substring(0, processName.LastIndexOf("."));
                foreach (Process item in ps)
                {
                    if (item.ProcessName.ToLower() == processName.ToLower())
                    {
                        item.Kill();
                    }
                }
            }
            catch (Exception e)
            {
            }
        }
        /// <summary>
        /// 确定指定的进程号是否存在
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static bool Exists(int pid)
        {
            if (pid == 0)
                return false;
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (pid == item.Id)
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 确定指定的进程名是否存在
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static bool Exists(string processName)
        {
            if (processName == "")
                return false;
            Process[] ps = Process.GetProcesses();
            foreach (Process item in ps)
            {
                if (item.ProcessName.ToLower() == processName.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
        /// <summary>
        /// 同步方法,等待外部程序运行完成并退出才返回
        /// </summary>
        /// <param name="file"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int StartSync(string file,string args)
        {
            ProcessStartInfo Info = new ProcessStartInfo(file, args);
            Info.WorkingDirectory = file.Substring(0, file.LastIndexOf("\\") + 1);
            Process pro = new Process();
            try
            {
                pro = Process.Start(Info);
                pro.WaitForExit();
            }
            catch(Exception e)
            {
                TOOL.LogTool.WriteLog(e.Message);
                return 0;
            }
            return pro.Id;
        }
        /// <summary>
        /// 异步方法,不等待外部程序执行完成
        /// </summary>
        /// <param name="file"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static int Start(string file, string args)
        {
            ProcessStartInfo Info = new ProcessStartInfo(file, args);
            Info.WorkingDirectory = file.Substring(0, file.LastIndexOf("\\") + 1);
            Process pro = new Process();
            try
            {
                pro = Process.Start(Info);
            }
            catch
            {
                return 0;
            }
            return pro.Id;
        }
    }
}

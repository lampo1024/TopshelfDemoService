using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace TopshelfDemoService
{
    /// <summary>
    /// 进程处理帮助类
    /// </summary>
    internal class ProcessorHelper
    {
        /// <summary>
        /// 获取当前计算机所有的进程列表(集合)
        /// </summary>
        /// <returns></returns>
        public static List<Process> GetProcessList()
        {
            return GetProcesses().ToList();
        }

        /// <summary>
        /// 获取当前计算机所有的进程列表(数组)
        /// </summary>
        /// <returns></returns>
        public static Process[] GetProcesses()
        {
            var processList = Process.GetProcesses();
            return processList;
        }

        /// <summary>
        /// 判断指定的进程是否存在
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static bool IsProcessExists(string processName)
        {
            return Process.GetProcessesByName(processName).Length > 0;
        }

        /// <summary>
        /// 启动一个指定路径的应用程序
        /// </summary>
        /// <param name="applicationPath"></param>
        /// <param name="args"></param>
        public static void RunProcess(string applicationPath, string args = "")
        {
            try
            {
                ProcessExtensions.StartProcessAsCurrentUser(applicationPath, args);
            }
            catch (Exception e)
            {
                var psi = new ProcessStartInfo
                {
                    FileName = applicationPath,
                    WindowStyle = ProcessWindowStyle.Normal,
                    Arguments = args
                };
                Process.Start(psi);
            }
        }
    }
}

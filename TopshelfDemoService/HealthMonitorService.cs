using System;
using System.Collections.Generic;
using System.Timers;

namespace TopshelfDemoService
{
    internal class HealthMonitorService
    {
        /// <summary>
        /// 检测周期计时器
        /// </summary>
        private readonly Timer _timer;
        /// <summary>
        /// 检测周期(秒)
        /// </summary>
        private int _monitorInterval = 3;
        /// <summary>
        /// 要守护的应用程序列表
        /// </summary>
        private List<DaemonApplicationInfo> _daemonApps { get; set; }

        public HealthMonitorService()
        {
            // 初始化要守护的应用程序列表
            // 实际项目中，你可以将这里的初始化参数放到配置文件/数据库/缓存中(怎么方便怎么来)
            _daemonApps = new List<DaemonApplicationInfo> {
                new DaemonApplicationInfo {
                    ProcessName ="TopshelfDemo.Client",	 // 请根据你的情况填写
                    AppDisplayName ="TopshelfDemo Client", // 请根据你的情况填写
                    AppFilePath =@"D:\Projects\github\TopshelfDemoService\TopshelfDemo.Client\bin\Debug\TopshelfDemo.Client.exe" // 请根据你的情况填写
                }
            };
            _timer = new Timer(_monitorInterval*1000) { AutoReset = true };
            _timer.Elapsed += (sender, eventArgs) => Monitor();
        }

        /// <summary>
        /// 守护应用程序的方法
        /// </summary>
        private void Monitor()
        {
            foreach (var app in _daemonApps)
            {
                // 判断当前进程是存已启动
                if (ProcessorHelper.IsProcessExists(app.ProcessName))
                {
                    Console.WriteLine("Application[{0}] already exists.", app.ProcessName);
                    return;
                }
                try
                {
                    // 当前主机进程列表中没有需要守护的进程名称，则启动这个进程对应的应用程序
                    ProcessorHelper.RunProcess(app.AppFilePath, app.Args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Start application failed:{0}", ex);
                }
            }            
        }

        public void Start()
        {
            _timer.Start();
        }
        public void Stop()
        {
            _timer.Stop();
        }
    }
}

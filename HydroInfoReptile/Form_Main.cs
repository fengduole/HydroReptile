using System;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using System.Net.NetworkInformation;

namespace HydroInfoReptile
{
    public partial class Form_Main : Form
    {
        const int net_try_time = 300 * 1000;            //网络检测等待时间(300秒)
        const int max_net_try_times = 24;               //最大网络检测次数
        const string main_page = "http://xxfb.hydroinfo.gov.cn/ssIndex.html"; //雨水情网站主页

        bool net_status = false;                        //网络状态，初始为false
        int net_try_times = 0;                          //网络状态检测尝试次数

        Class_Table[] _table = new Class_Table[4];      //数据表
        int _webFlag = 0;                               //浏览器加载进度标记
        string _html = "";                              //网页源代码
        int loadResult = 0;                             //是否成功获取数据 0 - 未知;1 - 成功;-1 - 失败, -2 - 存在失败

        Timer timer_netgateCheck;                       //检测网络状态
        Timer timer_loadDelay;                          //每500毫秒检查一次是否加载成功
        Timer timer_loadDeadline;                       //最长允许加载10分钟
        Timer timer_closeDelay;                         //延迟10秒关闭程序
        WebBrowser wb;                                  //网页加载器
        NotifyIcon notify;                              //托盘图标

        public Form_Main()
        {
            InitializeComponent();
        }


        

        /// <summary>
        /// 窗口加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Main_Load(object sender, EventArgs e)
        {
            Hide();
            panel_error.Visible = false;
            //WindowState = FormWindowState.Normal;

            WriteLog("ProgramStart", Application.ProductVersion);

            InitialControl();
            LoadSetting();

                       
            net_status = CheckNetStatus();
            net_try_times++;

            WriteLog("Netgate", net_status.ToString());
            PrintLog("Netgate", net_status.ToString());
            
            if (net_status) // 如果网络状态为true，则连接服务器
            {
                notify.BalloonTipText = "正在连接到服务器...";
                notify.Visible = true;
                notify.ShowBalloonTip(1000);
                WriteLog("Connenting", "");
                PrintLog("Connecting...", "");

                _webFlag = 1;
                loadResult = 0;
                wb.Navigate(main_page);                
            }
            else // 如果网络状态为false，则启动周期性网络检测
            {
                timer_netgateCheck.Start();
            }
        }

        /// <summary>
        /// 应用程序退出时
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            notify.Visible = false;
            WriteLog("ProgramStop", "");
            WriteLog("", "");
        }


        /// <summary>
        /// 浏览器加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void wb_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser wb = sender as WebBrowser;

            wb.ScriptErrorsSuppressed = true;

            if (wb.ReadyState != WebBrowserReadyState.Complete)
                return;

            if (_webFlag == 1)
            {
                notify.BalloonTipText = "正在获取数据...";
                notify.ShowBalloonTip(1000);

                PrintLog("Downloading...", "");

                _webFlag = 2;
                wb.Navigate("javascript:showSK();");

                timer_loadDelay.Start();
                timer_loadDeadline.Start();
            }
        }


        /// <summary>
        /// 该timer在程序开始时周期性执行，用以检测当前网络状态，若网络状态正常则开始加载雨水情网页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_netgateCheck_Tick(object sender, EventArgs e)
        {
            net_status = CheckNetStatus();
            net_try_times++;

            WriteLog("Netgate", net_status.ToString());
            PrintLog("Netgate", net_status.ToString());

            // 当前网络状态检测次数大于最大网络检测次数，则弹出窗口
            if (net_try_times >= max_net_try_times)
            {
                timer_netgateCheck.Stop();

                Show();
                WindowState = FormWindowState.Normal;

                return;
            }

            // 如果网络状态为false，则退出，等待下一次触发
            if (!net_status)
            {
                return;
            }

            notify.BalloonTipText = "正在连接到服务器...";
            notify.Visible = true;
            notify.ShowBalloonTip(1000);
            WriteLog("Connenting", "");
            PrintLog("Connecting...", "");

            _webFlag = 1;
            loadResult = 0;
            wb.Navigate(main_page);
        }


        /// <summary>
        /// 该timer被执行表明网页加载失败
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_loadDeadline_Tick(object sender, EventArgs e)
        {
            Timer timer = sender as Timer;

            timer.Stop();
            timer_loadDelay.Stop();
            loadResult = -1;

            WriteLog("DataDownload", "Timeout");
            PrintLog("DataDownload", "Failed");
            notify.BalloonTipText = "获取数据失败, 单击此处以重新获取.";
            notify.ShowBalloonTip(60000);
        }


        /// <summary>
        /// 检测是否加载到数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_loadDelay_Tick(object sender, EventArgs e)
        {
            Timer timer = sender as Timer;

            if (wb.Document.Body.InnerHtml.IndexOf("正在加载数据") >= 0)
                return;

            timer.Stop();
            timer_loadDeadline.Stop();
            WriteLog("DataDownload", "Successful");
            PrintLog("Download", "Successful");

            _html = wb.Document.Body.InnerHtml;

            StreamWriter sw = new StreamWriter(Class_Table._dataDir + DateTime.Now.ToString("yyyyMMdd") + ".html", false, Encoding.UTF8);
            sw.Write(_html);
            sw.Close();

            for (int i = 1; i <= 3; i++)
            {
                _table[i].ExtractFromHtml(_html);
            }

            if (_table[1]._loadStatus && _table[2]._loadStatus && _table[3]._loadStatus)
                loadResult = 1;
            else
                loadResult = -2;

            if (loadResult == 1)
            {
                System.Diagnostics.Process.Start("explorer.exe", Class_Table._dataDir);
                notify.BalloonTipText = "数据获取完毕!";
                notify.ShowBalloonTip(3000);
            }
            else if (loadResult == -2)
            {
                notify.BalloonTipText = "有未知错误发生, 请查看日志及源文件!";
                notify.ShowBalloonTip(60000);
                
                panel_error.Visible = true;
                Show();
                WindowState = FormWindowState.Normal;

                return;
            }

            timer_closeDelay.Start();
            PrintLog("Complete", "");
        }


        /// <summary>
        /// 延迟10秒关闭程序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void timer_closeDelay_Tick(object sender, EventArgs e)
        {
            Timer timer = sender as Timer;

            timer.Stop();

            Application.Exit();
        }


        /// <summary>
        /// 气泡被单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void notify_BalloonTipClicked(object sender, EventArgs e)
        {
            if (loadResult == 1)//成功获取数据 - 打开下载目录
            {
                System.Diagnostics.Process.Start("explorer.exe", Class_Table._dataDir);
                Application.Exit();
            }
            else if (loadResult == -1)//数据获取失败 - 重启
            {
                System.Diagnostics.Process.Start(Application.ExecutablePath);
                Application.Exit();
            }
            else if (loadResult == -2)//解译 或 储存错误 - 打开日志文件
            {
                System.Diagnostics.Process.Start("history.log");
                Application.Exit();
            }
        }

        /// <summary>
        /// 托盘图标被单击
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void notify_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }


        /// <summary>
        /// 气泡被关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void notify_BalloonTipClosed(object sender, EventArgs e)
        {
            if (loadResult == -1)
            {
                Application.Exit();
            }
        }

        //--------------------------------------------------------------
        //自定义函数
        //--------------------------------------------------------------

        /// <summary>
        /// 创建默认配置文件
        /// </summary>
        private void CreateSetting()
        {
            StreamWriter sw = new StreamWriter("option.ini", false, Encoding.Unicode);

            sw.WriteLine(Environment.CurrentDirectory + "\\HydroData\\");

            sw.Close();
        }


        /// <summary>
        /// 读取配置文件
        /// </summary>
        private void LoadSetting()
        {
            //不存在配置文件
            if (!File.Exists("option.ini"))
            {
                CreateSetting();
            }

            //不存在日志文件
            if (!File.Exists("history.log"))
            {
                File.Create("history.log");
            }

            //读取配置文件
            StreamReader sr = new StreamReader("option.ini", Encoding.Default);

            Class_Table._dataDir = sr.ReadLine();
            Directory.CreateDirectory(Class_Table._dataDir);

            sr.Close();

            //检查存放路径是否存在
            if (!Directory.Exists(Class_Table._dataDir))
            {
                Directory.CreateDirectory(Class_Table._dataDir);
            }
        }


        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitialControl()
        {
            //网页加载器
            wb = new WebBrowser();
            wb.DocumentCompleted += wb_DocumentCompleted;

            //每一段时间检测一次网络状态
            timer_netgateCheck = new Timer();
            timer_netgateCheck.Interval = net_try_time;
            timer_netgateCheck.Tick += timer_netgateCheck_Tick;

            //每500毫秒检查一次是否获取到数据
            timer_loadDelay = new Timer();
            timer_loadDelay.Interval = 500;
            timer_loadDelay.Tick += timer_loadDelay_Tick;

            //最长允许加载10分钟
            timer_loadDeadline = new Timer();
            timer_loadDeadline.Interval = 1000 * 60 * 10;
            timer_loadDeadline.Tick += timer_loadDeadline_Tick;

            //延迟10秒关闭程序
            timer_closeDelay = new Timer();
            timer_closeDelay.Interval = 1000 * 10;
            timer_closeDelay.Tick += timer_closeDelay_Tick;

            //初始化托盘图标
            notify = new NotifyIcon();
            notify.Icon = Properties.Resources.Macrofuns;
            notify.BalloonTipTitle = "全国雨水情数据下载器";
            notify.Text = "全国雨水情数据下载器";
            notify.BalloonTipClicked += notify_BalloonTipClicked;
            notify.Click += notify_Click;
            notify.BalloonTipClosed += notify_BalloonTipClosed;

            //初始化数据表
            _table[1] = new Class_Table("全国大型水库实时水情");
            _table[2] = new Class_Table("全国大江大河实时水情");
            _table[3] = new Class_Table("全国重点站实时雨情");
        }



        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="content">动作</param>
        /// <param name="status">状态</param>
        private void WriteLog(string content, string status)
        {
            StreamWriter sw = new StreamWriter("history.log", true, Encoding.Unicode);

            if (content == "" && status == "")
            {
                sw.WriteLine();
            }
            else
            {
                sw.WriteLine("{0}\t{1}\t{2}", DateTime.Now.ToString(), content, status);
            }

            sw.Close();
        }


        /// <summary>
        /// 将日志打印到屏幕
        /// </summary>
        /// <param name="content">动作</param>
        /// <param name="status">状态</param>
        private void PrintLog(string content, string status)
        {
            string log = string.Format("{0}\t{1}\t{2}\r\n", DateTime.Now.ToString(), content, status);

            textBox_log.Text += log;

            textBox_log.Select(0, 0);
        }


        /// <summary>
        /// 网络状态检测
        /// </summary>
        /// <param name="ping_url">需要ping的网址或IP地址，默认为百度主页</param>
        /// <returns>true为联网，否则为未联网</returns>
        public bool CheckNetStatus(string ping_url = "www.baidu.com")
        {
            try
            {
                Ping ping = new Ping();
                PingOptions ping_option = new PingOptions();
                byte[] buffer = Encoding.UTF8.GetBytes("");
                int timeout = 1000;

                ping_option.DontFragment = true;
                PingReply ping_reply = ping.Send(ping_url, timeout, buffer, ping_option);

                string info = ping_reply.Status.ToString();

                if (info == "Success")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        //--------------------------------------------------------------
        //错误报告
        //--------------------------------------------------------------

        /// <summary>
        /// 查看源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_html_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start("notepad++.exe", Class_Table._dataDir + DateTime.Now.ToString("yyyyMMdd") + ".html");
            }
            catch
            {
                Process.Start("notepad.exe", Class_Table._dataDir + DateTime.Now.ToString("yyyyMMdd") + ".html");
            }
        }


        /// <summary>
        /// 查看日志
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_log_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("notepad.exe", "history.log");
        }


        /// <summary>
        /// 查看输出目录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linkLabel_Data_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(Class_Table._dataDir);
        }
    }
}
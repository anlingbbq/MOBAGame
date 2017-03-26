using System;
using System.Collections.Generic;
using System.IO;
using Common.OpCode;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net;
using Photon.SocketServer;
using LogManager = ExitGames.Logging.LogManager;
using log4net.Config;
using MOBAServer.Handler;

namespace MOBAServer
{
    public class MobaServer : ApplicationBase
    {
        public static MobaServer Instance { get; private set; }

        // 保存所有对客户端请求的操作
        public Dictionary<OperationCode, BaseHandler> HandlerDict = new Dictionary<OperationCode, BaseHandler>();

        // 客户端连接
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            LogInfo("客户端连接");
            return new MobaPeer(initRequest);
        }

        // 服务器启动
        protected override void Setup()
        {
            Instance = this;

            InitLog();
            LogInfo("服务器开启");

            InitHandler();
        }

        // 服务器断开
        protected override void TearDown()
        {
            LogInfo("服务器关闭");
        }

        private void InitHandler()
        {
            // 创建所有handler对象并保存起来
            HandlerDict.Add(OperationCode.Login, new LoginHandler());
            HandlerDict.Add(OperationCode.Register, new RegisterHandler());
        }

        #region 日志功能
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        // 初始化日志
        private void InitLog()
        {
            // 指定使用的log插件
            LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
            // 指定日志文件存放位置
            GlobalContext.Properties["Photon:ApplicationLogPath"] = Path.Combine(
                Path.Combine(this.ApplicationRootPath, "bin_Win64"), "log");
            // 指定日志名称
            GlobalContext.Properties["LogFileName"] = this.ApplicationName;
            // 读取配置文件
            XmlConfigurator.ConfigureAndWatch(new FileInfo(Path.Combine(this.BinaryPath, "log4net.config")));
        }

        // 输出日志信息
        public static void LogInfo(string text)
        {
            log.Info(text);
        }

        // 输出错误信息
        public static void LogError(string text)
        {
            log.Error(text);
        }

        #endregion
    }
}

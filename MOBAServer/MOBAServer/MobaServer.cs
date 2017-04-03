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
using MOBAServer.Handler.Player;
using MOBAServer.Handler.Account;

namespace MOBAServer
{
    public class MobaServer : ApplicationBase
    {
        public static MobaServer Instance { get; private set; }

        // 保存所有对客户端请求的操作
        public Dictionary<OperationCode, BaseHandler> HandlerDict = new Dictionary<OperationCode, BaseHandler>();

        /// <summary>
        /// 客户端连接 
        /// </summary>
        /// <param name="initRequest"></param>
        /// <returns></returns>
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            LogInfo("客户端连接");
            return new MobaPeer(initRequest);
        }

        /// <summary>
        /// 服务器启动 
        /// </summary>
        protected override void Setup()
        {
            Instance = this;

            InitLog();
            LogInfo("服务器开启");

            InitHandler();
        }

        /// <summary>
        /// 服务器断开 
        /// </summary>
        protected override void TearDown()
        {
            LogInfo("服务器关闭");
        }

        /// <summary>
        /// 初始化处理类 
        /// </summary>
        private void InitHandler()
        {
            /** ================== 创建所有handler对象并保存起来 ================== **/

            // 用户操作
            HandlerDict.Add(OperationCode.UserLogin, new UserLoginHandler());
            HandlerDict.Add(OperationCode.UserRegister, new UserRegisterHandler());

            // 玩家操作
            HandlerDict.Add(OperationCode.PlayerCreate, new PlayerCreateHandler());
            HandlerDict.Add(OperationCode.PlayerGetInfo, new PlayerGetInfoHandler());
            HandlerDict.Add(OperationCode.PlayerOnline, new PlayerOnlineHandler());
            HandlerDict.Add(OperationCode.PlayerAddRequest, new PlayerAddRequestHandler());
            HandlerDict.Add(OperationCode.PlayerAddToClient, new PlayerAddToClientHandler());
            HandlerDict.Add(OperationCode.PlayerAddResult, new PlayerAddRequestHandler());
            HandlerDict.Add(OperationCode.StartMatch, new PlayerStartMatchHandler());
            HandlerDict.Add(OperationCode.StopMatch, new PlayerStopMatchHandler());
        }

        #region 日志功能
        private static readonly ILogger log = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 初始化日志 
        /// </summary>
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

        /// <summary>
        /// 输出日志信息 
        /// </summary>
        /// <param name="text"></param>
        public static void LogInfo(string text)
        {
            log.Info(text);
        }

        /// <summary>
        /// 输出警告信息 
        /// </summary>
        /// <param name="text"></param>
        public static void LogWarn(string text)
        {
            log.Warn(text);
        }

        /// <summary>
        /// 输出错误信息 
        /// </summary>
        /// <param name="text"></param>
        public static void LogError(string text)
        {
            log.Error(text);
        }

        #endregion
    }
}

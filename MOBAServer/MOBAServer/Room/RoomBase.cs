using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Code;
using Common.OpCode;
using ExitGames.Threading;
using NHibernate.SqlCommand;
using Photon.SocketServer;
using Photon.SocketServer.ServerToServer;

namespace MOBAServer.Room
{
    /// <summary>
    /// 房间基类
    /// </summary>
    /// <typeparam name="TPeer"></typeparam>
    public class RoomBase<TPeer>
        where TPeer : ClientPeer
    {
        /// <summary>
        /// 房间的id
        /// </summary>
        public int Id;

        /// <summary>
        /// 连接对象
        /// </summary>
        public List<TPeer> PeerList;

        /// <summary>
        /// 房间容量
        /// </summary>
        public int Count;

        /// <summary>
        /// 定时器
        /// </summary>
        public Timer Timer;

        /// <summary>
        /// 定时任务的id
        /// </summary>
        public Guid Guid;

        public RoomBase(int id, int count)
        {
            this.Id = id;
            this.Count = count;
            PeerList = new List<TPeer>();
            Guid = new Guid();
            Timer = new Timer();
            Timer.Start();
        }

        /// <summary>
        /// 进入房间
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        protected bool EnterRoom(TPeer peer)
        {
            if (PeerList.Contains(peer))
                return false;

            PeerList.Add(peer);
            return true;
        }

        /// <summary>
        /// 离开房间
        /// </summary>
        /// <param name="peer"></param>
        /// <returns></returns>
        protected bool LeaveRoom(TPeer peer)
        {
            if (PeerList.Contains(peer))
            {
                PeerList.Remove(peer);
                return true;
            }
                
            return false;
        }

        /// <summary>
        /// 开启定时任务
        /// </summary>
        /// <param name="utcTime"></param>
        /// <param name="callback"></param>
        public void StartSchedule(DateTime utcTime, Action callback)
        {
            this.Guid = Timer.AddAction(utcTime, callback);
        }

        /// <summary>
        /// 广播
        /// </summary>
        /// <param name="opCode">操作码</param>
        /// <param name="data">数据</param>
        /// <param name="self">自身的客户端连接</param>
        /// <param name="retCode">返回码</param>
        /// <param name="debugMsg">调试信息</param>
        public void Brocast(OperationCode opCode, Dictionary<byte, object> data, TPeer self = null,
            ReturnCode retCode = ReturnCode.Suceess, string debugMsg = "")
        {
            OperationResponse response = new OperationResponse((byte)opCode);
            response.Parameters = data;
            response.ReturnCode = (short) retCode;
            response.DebugMessage = debugMsg;

            foreach (TPeer peer in PeerList)
            {
                // 不给自身发送消息
                if (self == peer)
                    continue;

                peer.SendOperationResponse(response, new SendParameters());
            }
        }

        /// <summary>
        /// 广播事件
        /// </summary>
        /// <param name="opCode"></param>
        /// <param name="data"></param>
        /// <param name="self"></param>
        public void BrocastEvent(OperationCode opCode, Dictionary<byte, object> data, TPeer self = null)
        {
            EventData eventData = new EventData((byte)opCode);
            eventData.Parameters = data;

            foreach (TPeer peer in PeerList)
            {
                // 不给自身发送消息
                if (self == peer)
                    continue;

                peer.SendEvent(eventData, new SendParameters());
            }
        }
    }
}

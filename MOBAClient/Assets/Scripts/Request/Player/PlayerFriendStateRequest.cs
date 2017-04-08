using System;
using Common.OpCode;
using ExitGames.Client.Photon;

namespace Assets.Scripts.Request.Player
{
    /// <summary>
    /// 当好友上线或下线时的处理
    /// 不需要请求
    /// </summary>
    class PlayerFriendStateRequest : BaseRequest
    {
        private MainMenuPanel m_MainPanel;

        protected override void Start()
        {
            base.Start();
            m_MainPanel = GetComponent<MainMenuPanel>();
        }

        public override void OnOperationResponse(OperationResponse response)
        {
            m_MainPanel.OnFriendStateChange(response);
        }
    }
}

using Common.OpCode;
using ExitGames.Client.Photon;

public class PlayerOnlineRequest : BaseRequest
{
    private MainMenuPanel m_MainPanel;
    
    public override void Start()
    {
        OpCode = OperationCode.PlayerOnline;
        base.Start();

        m_MainPanel = GetComponent<MainMenuPanel>();
    }

    public override void DefalutRequest()
    {
        PhotonEngine.Peer.OpCustom((byte)OpCode, null, true);
    }

    /// <summary>
    /// 玩家上线的处理
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        m_MainPanel.OnOnline(response);
    }
}

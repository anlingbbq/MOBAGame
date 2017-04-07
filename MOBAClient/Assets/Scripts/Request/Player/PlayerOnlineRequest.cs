using Common.Code;
using Common.DTO;
using Common.OpCode;
using ExitGames.Client.Photon;
using LitJson;

public class PlayerOnlineRequest : BaseRequest
{
    private MainMenuPanel m_MainPanel;
    
    public override void Start()
    {
        base.Start();
        m_MainPanel = GetComponent<MainMenuPanel>();
    }

    /// <summary>
    /// 玩家上线的处理
    /// </summary>
    /// <param name="response"></param>
    public override void OnOperationResponse(OperationResponse response)
    {
        // 获取角色数据
        DtoPlayer dtoPlayer = JsonMapper.ToObject<DtoPlayer>
            (response.Parameters.ExTryGet((byte)ParameterCode.DtoPlayer) as string);

        GameData.player = dtoPlayer;
        m_MainPanel.OnOnline(dtoPlayer);
    }
}

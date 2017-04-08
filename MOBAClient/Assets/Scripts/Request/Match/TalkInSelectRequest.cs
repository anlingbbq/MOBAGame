using System.Collections.Generic;
using Common.Code;
using ExitGames.Client.Photon;

public class TalkInSelectRequest : BaseRequest
{
    private SelectPanel m_SelectPanel;

    protected override void Start()
    {
        base.Start();
        m_SelectPanel = GetComponent<SelectPanel>();
    }

    public void SendTalkRequesst(int teamId, string str)
    {
        Dictionary<byte, object> data = new Dictionary<byte, object>();
        data.Add((byte)ParameterCode.TeamId, teamId);
        data.Add((byte)ParameterCode.TalkContent, str);
        SendRequest(data);
    }

    public override void OnOperationResponse(OperationResponse response)
    {
        int teamId = (int)response.Parameters[(byte)ParameterCode.TeamId];
        string str = response.Parameters[(byte)ParameterCode.TalkContent] as string;

        m_SelectPanel.OnTalk(teamId, str);
    }
}
